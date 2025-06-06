using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// MovieActor repository
    /// </summary>
    public class MovieActorRepository : GenericRepository<MovieActor>, IMovieActorRepository
    {
        public MovieActorRepository(MovieDbContext context)
            : base(context) { }
    }
}