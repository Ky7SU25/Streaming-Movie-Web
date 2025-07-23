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
        Task<IEnumerable<string>> GetNamesByMovieIdAsync(int movieId);
        Task<IEnumerable<CategoryViewCountDto>> GetViewsByMovieCategoryAsync();
    }
    public class CategoryViewCountDto
    {
        public string CategoryName { get; set; }
        public int TotalViews { get; set; }
    }
}