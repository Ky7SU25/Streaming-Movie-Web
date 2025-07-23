using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Domain.Interfaces
{
    /// <summary>
    /// Interface for CategoryRepository
    /// </summary>
    public interface ICategoryRepository : IGenericRepository<Category> 
    {
        Task<IEnumerable<int>> GetCategoryIdsBySlugsAsync(IEnumerable<string> slugs);
    }
}