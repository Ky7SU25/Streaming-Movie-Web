namespace StreamingMovie.Application.Interfaces
{
    /// <summary>
    /// Interface for video upload service with ID reservation support
    /// </summary>
    public interface IVideoUploadService
    {
        // Main upload methods
        Task<object> UploadMovieAsync(object request);
        Task<object> UploadSeriesAsync(object request);
        Task<object> UploadEpisodeAsync(object request);
        
        // ID Reservation methods
        Task<int> ReserveMovieIdAsync(string title, string? slug = null);
        Task<int> ReserveSeriesIdAsync(string title, string? slug = null);
        Task<bool> CancelReservedIdAsync(string type, int id);
        
        // Helper methods
        Task<object> GetUploadStatsAsync();
        Task<object> GetSeriesListAsync();
        Task<bool> ValidateSlugAsync(string slug, string type, int? excludeId = null);
    }
}