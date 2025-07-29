using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StreamingMovie.Application.Common.Pagination;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Application.Services;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingRating.Application.Services
{
    public class RatingService : GenericService<Rating>, IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
         : base(unitOfWork.RatingRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<RatingResponseDTO>> PaginateBySlugAsync(string slug, int currentUserId, int page = 1, int pageSize = 7)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new ArgumentException("Slug cannot be null or empty.", nameof(slug));
            }

            var unifiedMovie = await _unitOfWork.UnifiedMovieRepository
                .FindOneAsync(x => x.Slug == slug);

            var query = _unitOfWork.RatingRepository.Find(r =>
                unifiedMovie.IsSeries ? r.SeriesId == unifiedMovie.Id : r.MovieId == unifiedMovie.Id);

            var totalCount = await query.CountAsync();

            var ratings = await query
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedRatings = _mapper.Map<List<RatingResponseDTO>>(ratings);

            foreach (var dto in mappedRatings)
            {
                var correspondingRating = ratings.First(r => r.Id == dto.Id);
                dto.IsCurrentUserReview = correspondingRating.UserId == currentUserId;
            }
            return new PagedResult<RatingResponseDTO>
            {
                Items = mappedRatings,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalCount
            };
        }

        public async Task<Rating> AddAsync(RatingRequestDTO request)
        {
            var unifiedMovie = await _unitOfWork.UnifiedMovieRepository
                .FindOneAsync(x => x.Slug == request.Slug);
            var rating = _mapper.Map<Rating>(request);

            if (unifiedMovie.IsSeries)
                rating.SeriesId = unifiedMovie.Id;
            else
                rating.MovieId = unifiedMovie.Id;

            return await _unitOfWork.RatingRepository.AddAsync(rating);
        }

        public async Task<Rating> UpdateAsync(RatingRequestDTO request)
        {
            var rating = await _unitOfWork.RatingRepository.FindOneAsync(r => r.Id == request.Id);

            if (rating == null)
            {
                throw new KeyNotFoundException("Rating not found.");
            }
            rating = _mapper.Map(request, rating);
            rating.UpdatedAt = DateTime.UtcNow;

            return await _unitOfWork.RatingRepository.UpdateAsync(rating);
        }

        public async Task<double> CalculateMovieRating(int movieId)
        {
            var ratings = await _unitOfWork.RatingRepository.Find(r => r.MovieId == movieId).ToListAsync();
            if (ratings.Count == 0)
            {
                return 0;
            }
            return Math.Round(ratings.Average(r => r.RatingValue), 1, MidpointRounding.AwayFromZero);
        }

        public async Task<double> CalculateSeriesRating(int seriesId)
        {
            var ratings = await _unitOfWork.RatingRepository.Find(r => r.SeriesId == seriesId).ToListAsync();
            if (ratings.Count == 0)
            {
                return 0;
            }
            return Math.Round(ratings.Average(r => r.RatingValue), 1, MidpointRounding.AwayFromZero);
        }

        public async Task<RatingResponseDTO?> GetUserReview(int userId, string slug) 
        {
            var unifiedMovie = await _unitOfWork.UnifiedMovieRepository
                .FindOneAsync(x => x.Slug == slug);
            if (unifiedMovie == null)
            {
                throw new KeyNotFoundException("Unified movie not found.");
            }
            var rating = await _unitOfWork.RatingRepository.FindOneAsync(r =>
                r.UserId == userId && (unifiedMovie.IsSeries ? r.SeriesId == unifiedMovie.Id : r.MovieId == unifiedMovie.Id));
            return _mapper.Map<RatingResponseDTO>(rating);
        }
    }
}
