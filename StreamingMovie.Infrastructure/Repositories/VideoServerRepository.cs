using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// VideoServer repository
    /// </summary>
    public class VideoServerRepository : GenericRepository<VideoServer>, IVideoServerRepository
    {
        public VideoServerRepository(MovieDbContext context)
            : base(context) { }
    }
}