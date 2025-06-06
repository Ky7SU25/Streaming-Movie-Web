using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// CommentReaction repository
    /// </summary>
    public class CommentReactionRepository : GenericRepository<CommentReaction>, ICommentReactionRepository
    {
        public CommentReactionRepository(MovieDbContext context)
            : base(context) { }
    }
}