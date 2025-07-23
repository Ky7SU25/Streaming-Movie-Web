using Microsoft.EntityFrameworkCore;
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

        public virtual async Task<List<Actor>> GetByMovieIdAsync(int movieId)
        {
            return await _dbSet
                .Where(ma => ma.MovieId == movieId)
                .Include(ma => ma.Actor)
                .Select(ma => ma.Actor)
                .ToListAsync();
        }
    }
}