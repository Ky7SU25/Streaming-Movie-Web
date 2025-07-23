using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public async Task<PagedResult<CommentResponseDTO>> PaginateBySlugAsync(string slug, int? episodeId, int page = 1, int pageSize = 5)
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

            return await query
                .Include(c => c.User)
                .ProjectTo<CommentResponseDTO>(_mapper.ConfigurationProvider)
                .ToPagedResultAsync(page, pageSize);
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

        public async Task<List<CommentResponseDTO>> GetRepliesAsync(int parentCommentId)
        {
            var replies = await _unitOfWork.CommentRepository
                .Find(c => c.ParentId == parentCommentId)
                .Include(c => c.User)
                .ProjectTo<CommentResponseDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return replies;
        }

    }
}
