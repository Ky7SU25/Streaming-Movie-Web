using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// SeriesCategory repository
    /// </summary>
    public class SeriesCategoryRepository : GenericRepository<SeriesCategory>, ISeriesCategoryRepository
    {
        public SeriesCategoryRepository(MovieDbContext context)
            : base(context) { }
    }
}