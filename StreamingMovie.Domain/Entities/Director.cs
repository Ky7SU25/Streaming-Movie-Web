using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for director table
    /// </summary>
    public class Director
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Biography { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(50)]
        public string? Nationality { get; set; }

        [MaxLength(255)]
        public string? AvatarUrl { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual IEnumerable<MovieDirector> MovieDirectors { get; set; }
        public virtual IEnumerable<SeriesDirector> SeriesDirectors { get; set; }
    }
}
