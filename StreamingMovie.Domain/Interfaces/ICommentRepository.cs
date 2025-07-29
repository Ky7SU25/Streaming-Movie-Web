using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Domain.Interfaces
{
    /// <summary>
    /// Interface for CommentRepository
    /// </summary>
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<bool> DeleteCommentWithChildrenAsync(int id);
    }
}