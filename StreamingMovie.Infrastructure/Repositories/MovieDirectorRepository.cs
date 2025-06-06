using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// MovieDirector repository
    /// </summary>
    public class MovieDirectorRepository : GenericRepository<MovieDirector>, IMovieDirectorRepository
    {
        public MovieDirectorRepository(MovieDbContext context)
            : base(context) { }
    }
}