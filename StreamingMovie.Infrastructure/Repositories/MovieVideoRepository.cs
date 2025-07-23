using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// MovieVideo repository
    /// </summary>
    public class MovieVideoRepository : GenericRepository<MovieVideo>, IMovieVideoRepository
    {
        public MovieVideoRepository(MovieDbContext context)
            : base(context) { }

        public async Task<MovieVideo> GetByMovieIdAsync(int movieId)
        {
            return await _dbSet
                .Include(mv => mv.Movie)
                .Include(mv => mv.VideoServer)
                .Include(mv => mv.VideoQuality)
                .FirstOrDefaultAsync(mv => mv.MovieId == movieId && mv.IsActive == true);
        }

        public async Task<List<MovieVideo>> GetAllByMovieIdAsync(int movieId)
        {
            return await _dbSet
                .Include(mv => mv.Movie)
                .Include(mv => mv.VideoServer)
                .Include(mv => mv.VideoQuality)
                .Where(mv => mv.MovieId == movieId && mv.IsActive == true)
                .OrderBy(mv => mv.VideoQuality.Resolution)
                .ToListAsync();
        }
    }
}
