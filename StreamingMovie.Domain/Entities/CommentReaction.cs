using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for comment_reaction table
    /// </summary>
    public class CommentReaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CommentId { get; set; }

        [Required]
        [MaxLength(10)]
        public string ReactionType { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public User User { get; set; }
        public Comment Comment { get; set; }
    }
}
