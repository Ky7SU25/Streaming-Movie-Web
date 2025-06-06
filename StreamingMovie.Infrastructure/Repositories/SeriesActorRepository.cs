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
    }
}