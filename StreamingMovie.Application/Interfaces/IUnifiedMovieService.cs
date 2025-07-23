using StreamingMovie.Application.Common.Pagination;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Domain.Entities;
using System.Linq.Expressions;

namespace StreamingMovie.Application.Interfaces
{
    public interface IUnifiedMovieService
    {
        Task<IEnumerable<UnifiedMovie>> GetAllAsync();
        //Query
        IQueryable<UnifiedMovie> Query(params Expression<Func<UnifiedMovie, object>>[] includes);
        // Predicate 
        IQueryable<UnifiedMovie> Find(params Expression<Func<UnifiedMovie, bool>>[] predicates);
        // Filter
        Task<PagedResult<UnifiedMovieDTO>> GetFilteredPagedMovies(MovieFilterDTO filter);
        // Get Movie Details
        Task<MovieDetailDTO?> GetMovieDetails(string slug);
    }
}
