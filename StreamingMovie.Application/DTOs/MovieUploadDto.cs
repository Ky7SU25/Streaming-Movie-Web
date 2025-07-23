using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Application.DTOs;

public class MovieUploadDto
{
    // Basic Information
    [Required(ErrorMessage = "Movie title is required")]
    [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Original title cannot exceed 255 characters")]
    public string? OriginalTitle { get; set; }

    [Required(ErrorMessage = "URL slug is required")]
    [StringLength(255, ErrorMessage = "Slug cannot exceed 255 characters")]
    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Duration must be a positive number")]
    public int? Duration { get; set; }

    public DateTime? ReleaseDate { get; set; }

    [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
    public string? Status { get; set; } = "active";

    public bool IsPremium { get; set; } = false;

    public int? CountryId { get; set; }

    // File URLs (after MinIO upload)
    public string? VideoUrl { get; set; }
    public string? PosterUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? SubtitleUrl { get; set; }
    public string? TrailerUrl { get; set; }

    // Video Settings
    public int VideoServerId { get; set; } = 1;
    public int VideoQualityId { get; set; } = 1;

    [StringLength(10, ErrorMessage = "Language code cannot exceed 10 characters")]
    public string? Language { get; set; } = "vi";

    // Categories & People
    public List<int>? CategoryIds { get; set; } = new List<int>();
    public List<int>? DirectorIds { get; set; } = new List<int>();
    public List<int>? ActorIds { get; set; } = new List<int>();

    // Reserved ID from reservation process
    public int? ReservedId { get; set; }
}