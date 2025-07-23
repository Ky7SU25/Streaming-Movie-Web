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

        public async Task<PagedResult<RatingResponseDTO>> PaginateBySlugAsync(string slug, int page = 1, int pageSize = 5)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new ArgumentException("Slug cannot be null or empty.", nameof(slug));
            }

            var unifiedMovie = await _unitOfWork.UnifiedMovieRepository
               .FindOneAsync(x => x.Slug == slug);

            var query = _unitOfWork.RatingRepository.Find(r =>
                unifiedMovie.IsSeries ? r.SeriesId == unifiedMovie.Id : r.MovieId == unifiedMovie.Id);

            return await query
                .Include(r => r.User)
                .ProjectTo<RatingResponseDTO>(_mapper.ConfigurationProvider)
                .OrderByDescending(r => r.CreatedAt)
                .ToPagedResultAsync(page, pageSize);
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
    }
}
