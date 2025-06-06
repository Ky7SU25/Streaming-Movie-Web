using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// Comment repository
    /// </summary>
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(MovieDbContext context)
            : base(context) { }
    }
}