using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for video_quality table
    /// </summary>
    public class VideoQuality
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Resolution { get; set; } = string.Empty;

        public int? Bitrate { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual IEnumerable<MovieVideo> MovieVideos { get; set; }
        public virtual IEnumerable<EpisodeVideo> EpisodeVideos { get; set; }
    }
}
