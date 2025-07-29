using StreamingMovie.Application.Common.Pagination;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;

namespace StreamingMovie.Application.Interfaces
{
    public interface ICommentService : IGenericService<Comment> 
    {
        Task<PagedResult<CommentResponseDTO>> PaginateBySlugAsync(string slug, int? episodeId, int page = 1, int pageSize = 5);
        Task<Comment> AddAsync(CommentRequestDTO request);
        Task<Comment> UpdateAsync(int commentId, string content);
        Task<bool> DeleteCommentWithChildrenAsync(int id);
        Task<List<CommentResponseDTO>> GetRepliesAsync(int parentCommentId);
    }
}
