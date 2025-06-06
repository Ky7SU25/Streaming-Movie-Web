using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// Banner repository
    /// </summary>
    public class BannerRepository : GenericRepository<Banner>, IBannerRepository
    {
        public BannerRepository(MovieDbContext context)
            : base(context) { }
    }
}