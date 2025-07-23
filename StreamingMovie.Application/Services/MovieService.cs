using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    public class MovieService : GenericService<Movie>, IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieService(IUnitOfWork unitOfWork)
            : base(unitOfWork.MovieRepository)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MovieVideoDTO> GetMovieVideoAsync(int movieId)
        {
            // Get the movie details first
            var movie = await _unitOfWork.MovieRepository.GetByIdAsync(movieId);
            if (movie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {movieId} not found.");
            }

            // Get all video qualities for this movie
            var movieVideos = await _unitOfWork.MovieVideoRepository.GetAllByMovieIdAsync(movieId);
            if (!movieVideos.Any())
            {
                throw new KeyNotFoundException($"No video found for movie with ID {movieId}.");
            }

            // Get the primary video (first one or highest quality)
            var primaryVideo = movieVideos.FirstOrDefault();

            var result = new MovieVideoDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                Slug = movie.Slug,
                Description = movie.Description,
                Thumbnail = movie.PosterUrl,
                VideoUrl = primaryVideo?.VideoUrl,
                VideoServer = primaryVideo?.VideoServer?.Name,
                VideoQuality = primaryVideo?.VideoQuality?.Name,
                SubtitleUrl = primaryVideo?.SubtitleUrl,
                Language = primaryVideo?.Language
            };

            // Generate quality-specific URLs
            result.QualityUrls = GenerateQualityUrls(primaryVideo?.VideoUrl);
            result.MasterPlaylistUrl = GenerateMasterPlaylistUrl(primaryVideo?.VideoUrl);

            // Map available qualities
            result.AvailableQualities = movieVideos
                .Select(mv => new VideoQualityInfo
                {
                    Id = mv.VideoQuality.Id,
                    Name = mv.VideoQuality.Name,
                    Resolution = mv.VideoQuality.Resolution,
                    Bitrate = mv.VideoQuality.Bitrate,
                    PlaylistUrl = GenerateQualitySpecificUrl(
                        mv.VideoUrl,
                        mv.VideoQuality.Resolution
                    )
                })
                .ToList();

            return result;
        }

        private Dictionary<string, string> GenerateQualityUrls(string? baseVideoUrl)
        {
            var qualityUrls = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(baseVideoUrl))
                return qualityUrls;

            // Remove the video.mp4 file and generate quality-specific URLs
            var baseUrl = RemoveVideoFileName(baseVideoUrl);

            // Standard qualities
            var qualities = new[] { "1080p", "720p", "480p", "360p" };

            foreach (var quality in qualities)
            {
                qualityUrls[quality] = $"{baseUrl}/{quality}.m3u8";
            }

            return qualityUrls;
        }

        private string? GenerateMasterPlaylistUrl(string? baseVideoUrl)
        {
            if (string.IsNullOrEmpty(baseVideoUrl))
                return null;

            var baseUrl = RemoveVideoFileName(baseVideoUrl);
            return $"{baseUrl}/master.m3u8";
        }

        private string GenerateQualitySpecificUrl(string baseVideoUrl, string resolution)
        {
            if (string.IsNullOrEmpty(baseVideoUrl))
                return string.Empty;

            var baseUrl = RemoveVideoFileName(baseVideoUrl);

            // Map resolution to quality folder name
            var qualityFolder = resolution switch
            {
                "1920x1080" => "1080p",
                "1280x720" => "720p",
                "854x480" => "480p",
                "640x360" => "360p",
                _
                    => resolution.Contains("1080")
                        ? "1080p"
                        : resolution.Contains("720")
                            ? "720p"
                            : resolution.Contains("480")
                                ? "480p"
                                : resolution.Contains("360")
                                    ? "360p"
                                    : "720p"
            };

            return $"{baseUrl}/{qualityFolder}.m3u8";
        }

        private string RemoveVideoFileName(string videoUrl)
        {
            // Remove video.mp4 or any video file extension from the URL
            // Example: https://minio.ngdat.io.vn/movies/movies/2025/73/video/video.mp4
            // Result: https://minio.ngdat.io.vn/movies/movies/2025/73/video

            var uri = new Uri(videoUrl);
            var pathWithoutFile = uri.AbsolutePath;

            // Remove the file name (video.mp4, etc.)
            var lastSlashIndex = pathWithoutFile.LastIndexOf('/');
            if (lastSlashIndex > 0)
            {
                pathWithoutFile = pathWithoutFile.Substring(0, lastSlashIndex);
            }

            return $"{uri.Scheme}://{uri.Host}{(uri.Port != 80 && uri.Port != 443 ? $":{uri.Port}" : "")}{pathWithoutFile}";
        }
    }
}
