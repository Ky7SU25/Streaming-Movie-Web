using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Application.DTOs;

public class SeriesUploadDto
{
    // Basic Information
    [Required(ErrorMessage = "Series title is required")]
    [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Original title cannot exceed 255 characters")]
    public string? OriginalTitle { get; set; }

    [Required(ErrorMessage = "URL slug is required")]
    [StringLength(255, ErrorMessage = "Slug cannot exceed 255 characters")]
    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Total seasons must be at least 1")]
    public int TotalSeasons { get; set; } = 1;

    public DateTime? ReleaseDate { get; set; }
    public DateTime? EndDate { get; set; }

    [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
    public string? Status { get; set; } = "active";

    public bool IsPremium { get; set; } = false;

    public int? CountryId { get; set; }

    // File URLs (after MinIO upload)
    public string? PosterUrl { get; set; }
    public string? BannerUrl { get; set; }

    // Categories & People
    public List<int>? CategoryIds { get; set; } = new List<int>();
    public List<int>? DirectorIds { get; set; } = new List<int>();
    public List<int>? ActorIds { get; set; } = new List<int>();

    // Reserved ID from reservation process
    public int? ReservedId { get; set; }
}