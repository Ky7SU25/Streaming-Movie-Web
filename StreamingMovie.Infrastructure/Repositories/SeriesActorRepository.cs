using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// SeriesActor repository
    /// </summary>
    public class SeriesActorRepository : GenericRepository<SeriesActor>, ISeriesActorRepository
    {
        public SeriesActorRepository(MovieDbContext context)
            : base(context) { }

        public virtual async Task<List<Actor>> GetBySeriesIdAsync(int seriesId)
        {
            return await _dbSet
                .Where(sa => sa.SeriesId == seriesId)
                .Include(sa => sa.Actor)
                .Select(sa => sa.Actor)
                .ToListAsync();
        }
    }
}