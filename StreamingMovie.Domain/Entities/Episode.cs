using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for episode table
    /// </summary>
    public class Episode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SeriesId { get; set; }

        [Required]
        public int SeasonNumber { get; set; }

        [Required]
        public int EpisodeNumber { get; set; }

        [MaxLength(255)]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public int? Duration { get; set; }

        public DateTime? AirDate { get; set; }

        public int? ViewCount { get; set; } = 0;

        public bool? IsPremium { get; set; } = false;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Series Series { get; set; }
        public virtual IEnumerable<EpisodeVideo> EpisodeVideos { get; set; }
        public virtual IEnumerable<WatchHistory> WatchHistories { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
    }
}
