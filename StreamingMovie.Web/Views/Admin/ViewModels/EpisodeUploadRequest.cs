using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Web.Views.Admin.ViewModels;

public class EpisodeUploadRequest
{
    // Episode Information
    [Required(ErrorMessage = "Series ID is required")]
    public int SeriesId { get; set; }

    [Required(ErrorMessage = "Season number is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Season number must be at least 1")]
    public int SeasonNumber { get; set; }

    [Required(ErrorMessage = "Episode number is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Episode number must be at least 1")]
    public int EpisodeNumber { get; set; }

    [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
    public string? Title { get; set; }

    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Duration must be a positive number")]
    public int? Duration { get; set; }

    public DateTime? AirDate { get; set; }

    public bool IsPremium { get; set; } = false;

    // File URLs (after S3/MinIO upload)
    public string? VideoUrl { get; set; }
    public string? SubtitleUrl { get; set; }

    // Video Settings
    public int VideoServerId { get; set; } = 1;
    public int VideoQualityId { get; set; } = 1;

    [StringLength(10, ErrorMessage = "Language code cannot exceed 10 characters")]
    public string? Language { get; set; } = "vi";
}