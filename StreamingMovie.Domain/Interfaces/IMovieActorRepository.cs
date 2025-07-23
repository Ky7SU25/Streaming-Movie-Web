using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Domain.Interfaces
{
    /// <summary>
    /// Interface for MovieActorRepository
    /// </summary>
    public interface IMovieActorRepository : IGenericRepository<MovieActor> 
    {
        Task<List<Actor>> GetByMovieIdAsync(int movieId);
    }
}