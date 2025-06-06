using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for watch_history table
    /// </summary>
    public class WatchHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? MovieId { get; set; }

        public int? EpisodeId { get; set; }

        public int? WatchPosition { get; set; } = 0;

        public int? WatchDuration { get; set; } = 0;

        public bool? IsCompleted { get; set; } = false;

        public DateTime? LastWatchedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public User User { get; set; }
        public Movie Movie { get; set; }
        public Episode Episode { get; set; }
    }
}
