using Microsoft.EntityFrameworkCore;
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
        public virtual async Task<Director> GetByMovieIdAsync(int movieId)
        {
            return await _dbSet
                .Where(md => md.MovieId == movieId)
                .Include(md => md.Director)
                .Select(md => md.Director)
                .FirstOrDefaultAsync(); 
        }
    }
}