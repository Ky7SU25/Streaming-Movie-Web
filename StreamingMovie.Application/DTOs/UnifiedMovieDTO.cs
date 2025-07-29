namespace StreamingMovie.Application.DTOs
{
    public class UnifiedMovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? OriginalTitle { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PosterUrl { get; set; }
        public string? BannerUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public float? ImdbRating { get; set; }
        public float? OurRating { get; set; }
        public int? ViewCount { get; set; }
        public string? Status { get; set; }
        public bool? IsPremium { get; set; }
        public int? CountryId { get; set; }
        public bool IsSeries { get; set; } 
    }

    public class TopViewMovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? PosterUrl { get; set; }
        public int? ViewCount { get; set; }
        public int? Duration { get; set; }
        public int? TotalEpisodes { get; set; }
        public bool IsSeries { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
