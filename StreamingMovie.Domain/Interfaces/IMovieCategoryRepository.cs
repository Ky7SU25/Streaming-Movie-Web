using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Domain.Interfaces
{
    /// <summary>
    /// Interface for MovieCategoryRepository
    /// </summary>
    public interface IMovieCategoryRepository : IGenericRepository<MovieCategory> 
    {
        Task<IEnumerable<int>> GetMovieIdsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<int>> GetMovieIdsByCategoryIdsAsync(IEnumerable<int> categoryIds);
    }
}