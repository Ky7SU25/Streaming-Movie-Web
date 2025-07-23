using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Domain.Interfaces
{
    /// <summary>
    /// Interface for SeriesDirectorRepository
    /// </summary>
    public interface ISeriesDirectorRepository : IGenericRepository<SeriesDirector>
    {
        Task<Director> GetBySeriesIdAsync(int seriesId);
    }
}