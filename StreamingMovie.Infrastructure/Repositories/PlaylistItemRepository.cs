using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// PlaylistItem repository
    /// </summary>
    public class PlaylistItemRepository : GenericRepository<PlaylistItem>, IPlaylistItemRepository
    {
        public PlaylistItemRepository(MovieDbContext context)
            : base(context) { }
    }
}