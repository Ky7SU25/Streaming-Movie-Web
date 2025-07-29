using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Domain.Interfaces
{
    /// <summary>
    /// Interface for MovieRepository
    /// </summary>
    public interface IMovieRepository : IGenericRepository<Movie> 
    {
        Task<int> GetTotalMovieAsync();
    }
}