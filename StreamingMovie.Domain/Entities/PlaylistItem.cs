using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for playlist_item table
    /// </summary>
    public class PlaylistItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PlaylistId { get; set; }

        public int? MovieId { get; set; }

        public int? SeriesId { get; set; }

        [Required]
        public int Position { get; set; }

        public DateTime? AddedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Playlist Playlist { get; set; }
        public Movie Movie { get; set; }
        public Series Series { get; set; }
    }
}
