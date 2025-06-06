using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for series table
    /// </summary>
    public class Series
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? OriginalTitle { get; set; }

        [Required]
        [MaxLength(255)]
        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(255)]
        public string? PosterUrl { get; set; }

        [MaxLength(255)]
        public string? BannerUrl { get; set; }

        [MaxLength(255)]
        public string? TrailerUrl { get; set; }

        public int? TotalSeasons { get; set; } = 1;

        public int? TotalEpisodes { get; set; } = 0;

        public DateTime? ReleaseDate { get; set; }

        public DateTime? EndDate { get; set; }

        public float? ImdbRating { get; set; }

        public float? OurRating { get; set; }

        public int? ViewCount { get; set; } = 0;

        [MaxLength(20)]
        public string? Status { get; set; }

        public bool? IsPremium { get; set; } = false;

        public int? CountryId { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Country Country { get; set; }
        public virtual IEnumerable<Episode> Episodes { get; set; }
        public virtual IEnumerable<SeriesCategory> SeriesCategories { get; set; }
        public virtual IEnumerable<SeriesDirector> SeriesDirectors { get; set; }
        public virtual IEnumerable<SeriesActor> SeriesActors { get; set; }
        public virtual IEnumerable<Favorite> Favorites { get; set; }
        public virtual IEnumerable<Rating> Ratings { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
        public virtual IEnumerable<PlaylistItem> PlaylistItems { get; set; }
        public virtual IEnumerable<Banner> Banners { get; set; }
    }
}
