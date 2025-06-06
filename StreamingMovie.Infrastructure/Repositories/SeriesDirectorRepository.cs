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
    }
}