using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for episode_video table
    /// </summary>
    public class EpisodeVideo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EpisodeId { get; set; }

        [Required]
        public int VideoServerId { get; set; }

        [Required]
        public int VideoQualityId { get; set; }

        [Required]
        [MaxLength(500)]
        public string VideoUrl { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? SubtitleUrl { get; set; }

        [MaxLength(10)]
        public string? Language { get; set; }

        public bool? IsActive { get; set; } = true;

        public string ProcessStatus { get; set; } = "pending";

        public string ErrorMessage { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Episode Episode { get; set; }
        public VideoServer VideoServer { get; set; }
        public VideoQuality VideoQuality { get; set; }
    }
}
