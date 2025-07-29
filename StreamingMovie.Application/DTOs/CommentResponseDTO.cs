using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Application.DTOs
{
    public class CommentResponseDTO
    {
        // comment information
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int? LikeCount { get; set; } = 0;
        public int? DislikeCount { get; set; } = 0;
        public bool? IsApproved { get; set; } = true;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // user information
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public bool isUserComment { get; set; }
    }
}
