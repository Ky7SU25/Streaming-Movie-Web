using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Domain.Interfaces
{
    /// <summary>
    /// Interface for MovieVideoRepository
    /// </summary>
    public interface IMovieVideoRepository : IGenericRepository<MovieVideo>
    {
        Task<MovieVideo> GetByMovieIdAsync(int movieId);
        Task<List<MovieVideo>> GetAllByMovieIdAsync(int movieId);
    }
}
