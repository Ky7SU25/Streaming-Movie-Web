using StreamingMovie.Application.Common.Pagination;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;

namespace StreamingMovie.Application.Interfaces
{
    public interface IRatingService : IGenericService<Rating> {

        Task<PagedResult<RatingResponseDTO>> PaginateBySlugAsync(string slug, int currentUserId, int page = 1, int pageSize = 7);
        Task<Rating> AddAsync(RatingRequestDTO request);
        Task<Rating> UpdateAsync(RatingRequestDTO request);
        Task<double> CalculateMovieRating(int movieId);
        Task<double> CalculateSeriesRating(int seriesId);
        Task<RatingResponseDTO?> GetUserReview(int userId, string slug);
    }
}
