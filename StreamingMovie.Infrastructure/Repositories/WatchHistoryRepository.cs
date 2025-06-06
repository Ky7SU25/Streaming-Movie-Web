using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// WatchHistory repository
    /// </summary>
    public class WatchHistoryRepository : GenericRepository<WatchHistory>, IWatchHistoryRepository
    {
        public WatchHistoryRepository(MovieDbContext context)
            : base(context) { }
    }
}