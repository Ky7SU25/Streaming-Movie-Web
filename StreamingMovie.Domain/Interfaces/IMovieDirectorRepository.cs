using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Domain.Interfaces
{
    /// <summary>
    /// Interface for MovieDirectorRepository
    /// </summary>
    public interface IMovieDirectorRepository : IGenericRepository<MovieDirector>
    {
        Task<Director> GetByMovieIdAsync(int movieId);
    }
}