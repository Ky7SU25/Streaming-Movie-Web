using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// SeriesDirector repository
    /// </summary>
    public class SeriesDirectorRepository : GenericRepository<SeriesDirector>, ISeriesDirectorRepository
    {
        public SeriesDirectorRepository(MovieDbContext context)
            : base(context) { }
        public virtual async Task<Director> GetBySeriesIdAsync(int seriesId)
        {
            return await _dbSet
                .Where(sd => sd.SeriesId == seriesId)
                .Include(sd => sd.Director)
                .Select(sd => sd.Director)
                .FirstOrDefaultAsync(); 
        }
    }
}