using StreamingMovie.Domain.Entities;
using System.Linq.Expressions;
namespace StreamingMovie.Domain.Interfaces
{
    public interface IUnifiedMovieRepository
    {
        Task<IEnumerable<UnifiedMovie>> GetAllAsync();
        //Query
        IQueryable<UnifiedMovie> Query(params Expression<Func<UnifiedMovie, object>>[] includes);
        // Predicate 
        IQueryable<UnifiedMovie> Find(params Expression<Func<UnifiedMovie, bool>>[] predicates);
    }
}
