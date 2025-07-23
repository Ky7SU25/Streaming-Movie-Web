using StreamingMovie.Application.Common.Pagination;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;

namespace StreamingMovie.Application.Interfaces
{
    public interface IRatingService : IGenericService<Rating> {

        Task<PagedResult<RatingResponseDTO>> PaginateBySlugAsync(string slug, int page = 1, int pageSize = 5);
        Task<Rating> AddAsync(RatingRequestDTO request);
        Task<Rating> UpdateAsync(RatingRequestDTO request);
    }
}
