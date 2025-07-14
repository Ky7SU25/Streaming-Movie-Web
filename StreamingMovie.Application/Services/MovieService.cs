using AutoMapper;
using AutoMapper.QueryableExtensions;
using StreamingMovie.Application.Common.Pagination;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Enums;
using StreamingMovie.Domain.UnitOfWorks;
using System.Linq.Expressions;

namespace StreamingMovie.Application.Services
{
    public class MovieService : GenericService<Movie>, IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork.MovieRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PagedResult<UnifiedMovieDTO>> GetPaginatedMovies(MovieFilterDTO filter)
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
                var countryIds = await _unitOfWork.CountryRepository.GetCountryIdsByCodesAsync(filter.Countries);
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
    }
}