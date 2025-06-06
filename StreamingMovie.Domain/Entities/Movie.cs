using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for movie table
    /// </summary>
    public class Movie
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

        public int? Duration { get; set; }

        public DateTime? ReleaseDate { get; set; }

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
        public virtual IEnumerable<MovieCategory> MovieCategories { get; set; }
        public virtual IEnumerable<MovieDirector> MovieDirectors { get; set; }
        public virtual IEnumerable<MovieActor> MovieActors { get; set; }
        public virtual IEnumerable<MovieVideo> MovieVideos { get; set; }
        public virtual IEnumerable<WatchHistory> WatchHistories { get; set; }
        public virtual IEnumerable<Favorite> Favorites { get; set; }
        public virtual IEnumerable<Rating> Ratings { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
        public virtual IEnumerable<PlaylistItem> PlaylistItems { get; set; }
        public virtual IEnumerable<Banner> Banners { get; set; }
    }
}
