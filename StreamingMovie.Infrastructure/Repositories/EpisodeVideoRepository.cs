using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// EpisodeVideo repository
    /// </summary>
    public class EpisodeVideoRepository : GenericRepository<EpisodeVideo>, IEpisodeVideoRepository
    {
        public EpisodeVideoRepository(MovieDbContext context)
            : base(context) { }
    }
}