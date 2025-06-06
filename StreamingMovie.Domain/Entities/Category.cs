using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for category table
    /// </summary>
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual IEnumerable<MovieCategory> MovieCategories { get; set; }
        public virtual IEnumerable<SeriesCategory> SeriesCategories { get; set; }
    }
}
