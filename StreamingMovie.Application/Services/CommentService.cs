using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StreamingMovie.Application.Common.Pagination;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    public class CommentService : GenericService<Comment>, ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
         : base(unitOfWork.CommentRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<CommentResponseDTO>> PaginateBySlugAsync(string slug, int? episodeId, int? currentUserId, int page = 1, int pageSize = 5)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new ArgumentException("Slug cannot be null or empty.", nameof(slug));
            }

            var unifiedMovie = await _unitOfWork.UnifiedMovieRepository
               .FindOneAsync(x => x.Slug == slug);

            if (unifiedMovie == null)
            {
                throw new KeyNotFoundException($"Unified movie with slug '{slug}' not found.");
            }

            if (unifiedMovie.IsSeries && episodeId == null)
            {
                throw new ArgumentException("EpisodeId cannot be null for series.", nameof(episodeId));
            }

            IQueryable<Comment> query;

            if (unifiedMovie.IsSeries)
            {
                if (!episodeId.HasValue)
                    throw new ArgumentException("EpisodeId required for series.");

                query = _unitOfWork.CommentRepository.Find(c =>
                    c.MovieId == null && c.EpisodeId == episodeId.Value && c.ParentId == null);
            }
            else
            {
                query = _unitOfWork.CommentRepository.Find(c =>
                    c.MovieId == unifiedMovie.Id && c.SeriesId == null && c.ParentId == null);
            }

            var totalCount = await query.CountAsync();

            var comments = await query
                .OrderByDescending(c => c.CreatedAt)
                .Include(c => c.User)
                .Take(pageSize)
                .ToListAsync();
;
            var mappedComments = _mapper.Map<List<CommentResponseDTO>>(comments);

            foreach (var dto in mappedComments)
            {
                var correspondingComment = comments.First(r => r.Id == dto.Id);
                dto.isUserComment = correspondingComment.UserId == currentUserId;
            }

            return new PagedResult<CommentResponseDTO>
            {
                Items = mappedComments,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalCount
            };
        }

        public async Task<Comment> AddAsync(CommentRequestDTO request)
        {
            var comment = _mapper.Map<Comment>(request);
            return await _unitOfWork.CommentRepository.AddAsync(comment);
        }

        public async Task<Comment> UpdateAsync(int commentId, string content)
        {
            var comment = await _unitOfWork.CommentRepository
                .FindOneAsync(x => x.Id == commentId);
            if (comment == null)
                throw new KeyNotFoundException("Comment not found.");

            comment.Content = content;
            comment.UpdatedAt = DateTime.UtcNow;

            return await _unitOfWork.CommentRepository.UpdateAsync(comment);
        }

        public async Task<bool> DeleteCommentWithChildrenAsync(int id)
        {
            return await _unitOfWork.CommentRepository.DeleteCommentWithChildrenAsync(id);
        }

        public async Task<List<CommentResponseDTO>> GetRepliesAsync(int parentCommentId, int? currentUserId)
        {
            var replies = await _unitOfWork.CommentRepository
                .Find(c => c.ParentId == parentCommentId)
                .OrderByDescending(c => c.CreatedAt)
                .Include(c => c.User)
                .ToListAsync();

            var mappedComments = _mapper.Map<List<CommentResponseDTO>>(replies);

            foreach (var dto in mappedComments)
            {
                var correspondingComment = replies.First(r => r.Id == dto.Id);
                dto.isUserComment = correspondingComment.UserId == currentUserId;
            }

            return mappedComments;
        }
    }
}
