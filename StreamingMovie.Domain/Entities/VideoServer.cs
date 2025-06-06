using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for video_server table
    /// </summary>
    public class VideoServer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string BaseUrl { get; set; } = string.Empty;

        public bool? IsActive { get; set; } = true;

        public int? Priority { get; set; } = 1;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual IEnumerable<MovieVideo> MovieVideos { get; set; }
        public virtual IEnumerable<EpisodeVideo> EpisodeVideos { get; set; }
    }
}
