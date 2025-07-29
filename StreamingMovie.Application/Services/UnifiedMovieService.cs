using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StreamingMovie.Application.Common.Pagination;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Application.Interfaces.ExternalServices.Huggingface;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Enums;
using StreamingMovie.Domain.UnitOfWorks;
using System.Linq.Expressions;
using System.Text.Json;

namespace StreamingMovie.Application.Services
{
    public class UnifiedMovieService : IUnifiedMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmbeddingService _embeddingService;


        public UnifiedMovieService(IUnitOfWork unitOfWork, IMapper mapper, IEmbeddingService embeddingService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _embeddingService = embeddingService;
        }

        public async Task<IEnumerable<UnifiedMovie>> GetAllAsync()
        {
            return await _unitOfWork.UnifiedMovieRepository.GetAllAsync();
        }

        public IQueryable<UnifiedMovie> Query(params Expression<Func<UnifiedMovie, object>>[] includes)
        {
            return _unitOfWork.UnifiedMovieRepository.Query(includes);
        }

        public IQueryable<UnifiedMovie> Find(params Expression<Func<UnifiedMovie, bool>>[] predicates)
        {
            return _unitOfWork.UnifiedMovieRepository.Find(predicates);
        }

        public async Task<PagedResult<UnifiedMovieDTO>> GetFilteredPagedMovies(MovieFilterDTO filter)
        {
            var predicates = new List<Expression<Func<UnifiedMovie, bool>>>();

            // Keyword
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                predicates.Add(x =>
                    x.Slug.Contains(filter.Keyword) ||
                    x.Title.Contains(filter.Keyword) ||
                    (x.Description != null && x.Description.Contains(filter.Keyword)) ||
                    (x.OriginalTitle != null && x.OriginalTitle.Contains(filter.Keyword)));
            }

            // Country
            if (filter.Countries?.Any(x => !string.IsNullOrWhiteSpace(x)) == true)
            {
                var countryIds = await _unitOfWork.CountryRepository.GetIdsByCodesAsync(filter.Countries);
                predicates.Add(x => x.CountryId.HasValue && countryIds.Contains(x.CountryId.Value));
            }

            // Year
            if (filter.Years?.Any() == true)
            {
                predicates.Add(x => x.ReleaseDate.HasValue && filter.Years.Contains(x.ReleaseDate.Value.Year));
            }

            // Type
            if (filter.Type == MovieType.Movie)
            {
                predicates.Add(x => x.IsSeries == false);
            }
            else if (filter.Type == MovieType.Series)
            {
                predicates.Add(x => x.IsSeries == true);
            }

            // Status
            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                predicates.Add(x => x.Status == filter.Status);
            }

            // Query
            var query = _unitOfWork.UnifiedMovieRepository.Find(predicates.ToArray());

            // Category 
            if (filter.Categories?.Any(x => !string.IsNullOrWhiteSpace(x)) == true)
            {
                var categoryIds = await _unitOfWork.CategoryRepository
                    .GetCategoryIdsBySlugsAsync(filter.Categories);

                var movieIds = await _unitOfWork.MovieCategoryRepository
                    .GetMovieIdsByCategoryIdsAsync(categoryIds);

                var seriesIds = await _unitOfWork.SeriesCategoryRepository
                    .GetSeriesIdsByCategoryIdsAsync(categoryIds);

                query = query.Where(x =>
                    (!x.IsSeries && movieIds.Contains(x.Id)) ||
                    (x.IsSeries && seriesIds.Contains(x.Id)));
            }

            // Sort
            query = filter.OrderBy switch
            {
                MovieSortOption.Newest => query.OrderByDescending(x => x.ReleaseDate),
                MovieSortOption.RecentlyUpdated => query.OrderByDescending(x => x.UpdatedAt),
                MovieSortOption.ImdbRating => query.OrderByDescending(x => x.ImdbRating),
                MovieSortOption.ViewCount => query.OrderByDescending(x => x.ViewCount),
                _ => query.OrderByDescending(x => x.ReleaseDate)
            };

            // Project and Paginate
            var dtoQuery = query.ProjectTo<UnifiedMovieDTO>(_mapper.ConfigurationProvider);
            return await dtoQuery.ToPagedResultAsync(filter.Page, filter.PageSize);
        }

        public async Task<MovieDetailDTO?> GetMovieDetails(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new ArgumentException("Slug cannot be null or empty.", nameof(slug));
            }

            var unifiedMovie = await _unitOfWork.UnifiedMovieRepository
                .FindOneAsync(x => x.Slug == slug);
        
            var detailDto = _mapper.Map<MovieDetailDTO>(unifiedMovie);

            detailDto.Movies = (await _unitOfWork.MovieRepository.GetAllAsync()).OrderByDescending(m => m.CreatedAt)
                .Take(5);
            detailDto.Language = await _unitOfWork.CountryRepository.GetNameByIdAsync(unifiedMovie.CountryId);

            detailDto.Genres = unifiedMovie.IsSeries
                ? await _unitOfWork.SeriesCategoryRepository.GetNameBySeriesIdAsync(unifiedMovie.Id)
                : await _unitOfWork.MovieCategoryRepository.GetNamesByMovieIdAsync(unifiedMovie.Id);

            return detailDto;
        }

        public async Task<PagedResult<UnifiedMovieDTO>> GetAISearchPagedMovies(string query)
        {
            var queryEmbedding = await _embeddingService.GetEmbeddingAsync(query);
            var movies = await _unitOfWork.UnifiedMovieRepository.Find(m => m.IsSeries == false).ToListAsync();
            var result = movies
            .Select(m => new
            {
                Movie = m,
                Score = CosineSimilarity(
                    queryEmbedding,
                    JsonSerializer.Deserialize<float[]>(m.EmbeddingJson) ?? new float[0]
                )
            })
            .Where(x => x.Score >= 0.3f) // lọc những phim có điểm số tương đồng tối thiểu
            .OrderByDescending(x => x.Score)
            .Take(10)
            .Select(x => x.Movie)
            .ToList();
            var pagedResult = new PagedResult<UnifiedMovieDTO>
            {
                Items = _mapper.Map<IEnumerable<UnifiedMovieDTO>>(result),
                TotalItems = result.Count,
                PageSize = 10,
                Page = 1
            };
            return pagedResult;
        }

        private float CosineSimilarity(float[] vec1, float[] vec2)
        {
            if (vec1.Length != vec2.Length || vec1.Length == 0)
                return 0;

            float dot = 0, mag1 = 0, mag2 = 0;
            for (int i = 0; i < vec1.Length; i++)
            {
                dot += vec1[i] * vec2[i];
                mag1 += vec1[i] * vec1[i];
                mag2 += vec2[i] * vec2[i];
            }
            return dot / ((float)Math.Sqrt(mag1) * (float)Math.Sqrt(mag2));
        }
    }
}
