using StreamingMovie.Application.Common.Pagination;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;

namespace StreamingMovie.Application.Interfaces
{
    public interface IMovieService : IGenericService<Movie>
    {
        Task<PagedResult<UnifiedMovieDTO>> GetPaginatedMovies(MovieFilterDTO filter);
    }
}
