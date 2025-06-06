using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// VideoQuality repository
    /// </summary>
    public class VideoQualityRepository : GenericRepository<VideoQuality>, IVideoQualityRepository
    {
        public VideoQualityRepository(MovieDbContext context)
            : base(context) { }
    }
}