using Microsoft.EntityFrameworkCore;
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

        public virtual async Task<bool> DeleteCommentWithChildrenAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                    return false;

                await DeleteCommentWithChildren(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.Error.WriteLine($"Constraint error in deleting or not exist: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }

        private async Task DeleteCommentWithChildren(Comment comment)
        {
            var childComments = await _dbContext.Comments
                .Where(c => c.ParentId == comment.Id).ToListAsync();

            foreach (var childComment in childComments)
            {
                await DeleteCommentWithChildren(childComment);
            }

            _dbContext.Comments.Remove(comment);
        }
    }
}