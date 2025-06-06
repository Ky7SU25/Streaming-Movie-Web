using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for comment table
    /// </summary>
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? MovieId { get; set; }

        public int? SeriesId { get; set; }

        public int? EpisodeId { get; set; }

        public int? ParentId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public int? LikeCount { get; set; } = 0;

        public int? DislikeCount { get; set; } = 0;

        public bool? IsApproved { get; set; } = true;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Series Series { get; set; }
        public virtual Episode Episode { get; set; }
        public virtual Comment Parent { get; set; }
        public virtual IEnumerable<Comment> Replies { get; set; }
        public virtual IEnumerable<CommentReaction> CommentReactions { get; set; }
    }
}
