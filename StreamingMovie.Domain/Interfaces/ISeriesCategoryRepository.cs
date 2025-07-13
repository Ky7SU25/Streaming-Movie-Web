using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Domain.Interfaces
{
    /// <summary>
    /// Interface for SeriesCategoryRepository
    /// </summary>
    public interface ISeriesCategoryRepository : IGenericRepository<SeriesCategory>
    {
        Task<IEnumerable<int>> GetSeriesIdsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<int>> GetSeriesIdsByCategoryIdsAsync(IEnumerable<int> categoryIds);
    }
}