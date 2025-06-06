using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// Rating repository
    /// </summary>
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        public RatingRepository(MovieDbContext context)
            : base(context) { }
    }
}