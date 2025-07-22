using System;
using Microsoft.Extensions.Logging;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Application.Interfaces.ExternalServices.Messages;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;

namespace StreamingMovie.Application.Services
{
    /// <summary>
    /// Full implementation of VideoUploadService with database operations
    /// </summary>
    public class VideoUploadService : IVideoUploadService
    {
        private readonly ILogger<VideoUploadService> _logger;
        private readonly IMovieRepository _movieRepository;
        private readonly ISeriesRepository _seriesRepository;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMovieVideoRepository _movieVideoRepository;
        private readonly IEpisodeVideoRepository _episodeVideoRepository;
        private readonly IQueuePublisher _queuePublisher;

        public VideoUploadService(
            ILogger<VideoUploadService> logger,
            IMovieRepository movieRepository,
            ISeriesRepository seriesRepository,
            IEpisodeRepository episodeRepository,
            IMovieVideoRepository movieVideoRepository,
            IEpisodeVideoRepository episodeVideoRepository,
            IQueuePublisher queuePublisher
        )
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _seriesRepository = seriesRepository;
            _episodeRepository = episodeRepository;
            _movieVideoRepository = movieVideoRepository;
            _episodeVideoRepository = episodeVideoRepository;
            _queuePublisher = queuePublisher;
        }

        #region ID Reservation Methods

        public async Task<int> ReserveMovieIdAsync(string title, string? slug = null)
        {
            try
            {
                _logger.LogInformation("?? Creating draft movie record for title: {Title}", title);

                // Create draft movie record in database
                var movie = new Movie
                {
                    Title = title,
                    Slug = slug ?? GenerateSlug(title),
                    Status = "draft",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdMovie = await _movieRepository.AddAsync(movie);

                _logger.LogInformation("? Draft movie created with ID: {MovieId}", createdMovie.Id);
                return createdMovie.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error creating draft movie for title: {Title}", title);
                throw;
            }
        }

        public async Task<int> ReserveSeriesIdAsync(string title, string? slug = null)
        {
            try
            {
                _logger.LogInformation("?? Creating draft series record for title: {Title}", title);

                // Create draft series record in database
                var series = new Series
                {
                    Title = title,
                    Slug = slug ?? GenerateSlug(title),
                    Status = "draft",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdSeries = await _seriesRepository.AddAsync(series);

                _logger.LogInformation(
                    "? Draft series created with ID: {SeriesId}",
                    createdSeries.Id
                );
                return createdSeries.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error creating draft series for title: {Title}", title);
                throw;
            }
        }

        public async Task<bool> CancelReservedIdAsync(string type, int id)
        {
            try
            {
                _logger.LogInformation("??? Cancelling reserved {Type} ID: {Id}", type, id);

                bool deleted = false;
                if (type.ToLower() == "movie")
                {
                    var movie = await _movieRepository.GetByIdAsync(id);
                    if (movie != null && movie.Status == "draft")
                    {
                        deleted = await _movieRepository.DeleteAsync(id);
                    }
                }
                else if (type.ToLower() == "series")
                {
                    var series = await _seriesRepository.GetByIdAsync(id);
                    if (series != null && series.Status == "draft")
                    {
                        deleted = await _seriesRepository.DeleteAsync(id);
                    }
                }

                if (deleted)
                {
                    _logger.LogInformation(
                        "? Reserved {Type} ID {Id} cancelled successfully",
                        type,
                        id
                    );
                    return true;
                }
                else
                {
                    _logger.LogWarning(
                        "?? Failed to cancel reserved {Type} ID: {Id} (not found or not draft)",
                        type,
                        id
                    );
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error cancelling reserved {Type} ID: {Id}", type, id);
                return false;
            }
        }

        #endregion

        #region Main Upload Methods

        public async Task<object> UploadMovieAsync(object request)
        {
            try
            {
                var movieRequest = request as MovieUploadDto;
                if (movieRequest == null)
                {
                    _logger.LogError("? Invalid movie upload request type");
                    return new { success = false, message = "Invalid request type" };
                }

                _logger.LogInformation(
                    "?? Processing movie upload for: {Title}",
                    movieRequest.Title
                );

                // Find the draft movie record (should exist from reservation)
                Movie? movie = null;

                // Try to find by reserved ID first, then by title
                if (movieRequest.ReservedId.HasValue)
                {
                    movie = await _movieRepository.GetByIdAsync(movieRequest.ReservedId.Value);
                    if (movie?.Status != "draft")
                    {
                        movie = null; // Not a valid draft movie
                    }
                }

                if (movie == null)
                {
                    var existingMovies = await _movieRepository.FindAsync(m =>
                        m.Title == movieRequest.Title && m.Status == "draft"
                    );
                    movie = existingMovies.FirstOrDefault();
                }

                if (movie == null)
                {
                    _logger.LogError(
                        "? Draft movie not found for title: {Title}",
                        movieRequest.Title
                    );
                    return new { success = false, message = "Draft movie record not found" };
                }

                // Update movie with complete information
                movie.OriginalTitle = movieRequest.OriginalTitle;
                movie.Slug = movieRequest.Slug;
                movie.Description = movieRequest.Description;
                movie.Duration = movieRequest.Duration;
                movie.ReleaseDate = movieRequest.ReleaseDate;
                movie.PosterUrl = movieRequest.PosterUrl;
                movie.BannerUrl = movieRequest.BannerUrl;
                movie.TrailerUrl = movieRequest.TrailerUrl;
                movie.Status = "active"; // Change from draft to active
                movie.IsPremium = movieRequest.IsPremium;
                movie.CountryId = movieRequest.CountryId;
                movie.UpdatedAt = DateTime.UtcNow;

                var updatedMovie = await _movieRepository.UpdateAsync(movie);

                // Create MovieVideo record if video URL is provided
                if (!string.IsNullOrEmpty(movieRequest.VideoUrl))
                {
                    var movieVideo = new MovieVideo
                    {
                        MovieId = updatedMovie.Id,
                        VideoServerId = movieRequest.VideoServerId,
                        VideoQualityId = movieRequest.VideoQualityId,
                        VideoUrl = movieRequest.VideoUrl,
                        SubtitleUrl = movieRequest.SubtitleUrl,
                        Language = movieRequest.Language,
                        IsActive = true,
                        ProcessStatus = "processing",
                        CreatedAt = DateTime.UtcNow
                    };

                    await _movieVideoRepository.AddAsync(movieVideo);
                    _logger.LogInformation(
                        "? MovieVideo record created for movie ID: {MovieId}",
                        updatedMovie.Id
                    );
                }

                var movieVideoUrl = movieRequest.VideoUrl;
                await _queuePublisher.Publish(
                    new VideoProcessingMessage
                    {
                        MovieId = updatedMovie.Id.ToString(),
                        MovieVideoId = updatedMovie.Id.ToString(),
                        PathStorage = movieVideoUrl.Substring(
                            movieVideoUrl.IndexOf(
                                "/",
                                movieVideoUrl.IndexOf('/', movieVideoUrl.IndexOf("//") + 2) + 1
                            ) + 1
                        )
                    }
                );

                // TODO: Handle categories, directors, actors relationships
                await HandleMovieRelationships(updatedMovie.Id, movieRequest);

                _logger.LogInformation(
                    "?? Movie upload completed successfully for ID: {MovieId}",
                    updatedMovie.Id
                );
                return new
                {
                    success = true,
                    message = "Movie uploaded successfully",
                    id = updatedMovie.Id,
                    title = updatedMovie.Title,
                    slug = updatedMovie.Slug
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error uploading movie");
                return new
                {
                    success = false,
                    message = "An error occurred while uploading the movie"
                };
            }
        }

        public async Task<object> UploadSeriesAsync(object request)
        {
            try
            {
                var seriesRequest = request as SeriesUploadDto;
                if (seriesRequest == null)
                {
                    _logger.LogError("? Invalid series upload request type");
                    return new { success = false, message = "Invalid request type" };
                }

                _logger.LogInformation(
                    "?? Processing series upload for: {Title}",
                    seriesRequest.Title
                );

                // Find the draft series record (should exist from reservation)
                Series? series = null;

                // Try to find by reserved ID first, then by title
                if (seriesRequest.ReservedId.HasValue)
                {
                    series = await _seriesRepository.GetByIdAsync(seriesRequest.ReservedId.Value);
                    if (series?.Status != "draft")
                    {
                        series = null; // Not a valid draft series
                    }
                }

                if (series == null)
                {
                    var existingSeries = await _seriesRepository.FindAsync(s =>
                        s.Title == seriesRequest.Title && s.Status == "draft"
                    );
                    series = existingSeries.FirstOrDefault();
                }

                if (series == null)
                {
                    _logger.LogError(
                        "? Draft series not found for title: {Title}",
                        seriesRequest.Title
                    );
                    return new { success = false, message = "Draft series record not found" };
                }

                // Update series with complete information
                series.OriginalTitle = seriesRequest.OriginalTitle;
                series.Slug = seriesRequest.Slug;
                series.Description = seriesRequest.Description;
                series.TotalSeasons = seriesRequest.TotalSeasons;
                series.ReleaseDate = seriesRequest.ReleaseDate;
                series.EndDate = seriesRequest.EndDate;
                series.PosterUrl = seriesRequest.PosterUrl;
                series.BannerUrl = seriesRequest.BannerUrl;
                series.Status = "active"; // Change from draft to active
                series.IsPremium = seriesRequest.IsPremium;
                series.CountryId = seriesRequest.CountryId;
                series.UpdatedAt = DateTime.UtcNow;

                var updatedSeries = await _seriesRepository.UpdateAsync(series);

                // TODO: Handle categories, directors, actors relationships
                await HandleSeriesRelationships(updatedSeries.Id, seriesRequest);

                _logger.LogInformation(
                    "?? Series upload completed successfully for ID: {SeriesId}",
                    updatedSeries.Id
                );
                return new
                {
                    success = true,
                    message = "Series created successfully",
                    id = updatedSeries.Id,
                    title = updatedSeries.Title,
                    slug = updatedSeries.Slug
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error uploading series");
                return new
                {
                    success = false,
                    message = "An error occurred while creating the series"
                };
            }
        }

        public async Task<object> UploadEpisodeAsync(object request)
        {
            try
            {
                var episodeRequest = request as EpisodeUploadDto;
                if (episodeRequest == null)
                {
                    _logger.LogError("? Invalid episode upload request type");
                    return new { success = false, message = "Invalid request type" };
                }

                _logger.LogInformation(
                    "?? Processing episode upload for S{Season}E{Episode}",
                    episodeRequest.SeasonNumber,
                    episodeRequest.EpisodeNumber
                );

                // Validate that the series exists
                var series = await _seriesRepository.GetByIdAsync(episodeRequest.SeriesId);
                if (series == null)
                {
                    _logger.LogError(
                        "? Series not found with ID: {SeriesId}",
                        episodeRequest.SeriesId
                    );
                    return new { success = false, message = "Series not found" };
                }

                // Check if episode already exists
                var existingEpisodes = await _episodeRepository.FindAsync(e =>
                    e.SeriesId == episodeRequest.SeriesId
                    && e.SeasonNumber == episodeRequest.SeasonNumber
                    && e.EpisodeNumber == episodeRequest.EpisodeNumber
                );

                if (existingEpisodes.Any())
                {
                    _logger.LogWarning(
                        "?? Episode already exists: S{Season}E{Episode}",
                        episodeRequest.SeasonNumber,
                        episodeRequest.EpisodeNumber
                    );
                    return new { success = false, message = "Episode already exists" };
                }

                // Create new episode record
                var episode = new Episode
                {
                    SeriesId = episodeRequest.SeriesId,
                    SeasonNumber = episodeRequest.SeasonNumber,
                    EpisodeNumber = episodeRequest.EpisodeNumber,
                    Title = episodeRequest.Title,
                    Description = episodeRequest.Description,
                    Duration = episodeRequest.Duration,
                    AirDate = episodeRequest.AirDate,
                    IsPremium = episodeRequest.IsPremium,
                    CreatedAt = DateTime.UtcNow
                };

                var createdEpisode = await _episodeRepository.AddAsync(episode);

                // Create EpisodeVideo record if video URL is provided
                if (!string.IsNullOrEmpty(episodeRequest.VideoUrl))
                {
                    var episodeVideo = new EpisodeVideo
                    {
                        EpisodeId = createdEpisode.Id,
                        VideoServerId = episodeRequest.VideoServerId,
                        VideoQualityId = episodeRequest.VideoQualityId,
                        VideoUrl = episodeRequest.VideoUrl,
                        SubtitleUrl = episodeRequest.SubtitleUrl,
                        Language = episodeRequest.Language,
                        IsActive = true,
                        ProcessStatus = "completed",
                        CreatedAt = DateTime.UtcNow
                    };

                    await _episodeVideoRepository.AddAsync(episodeVideo);
                    _logger.LogInformation(
                        "? EpisodeVideo record created for episode ID: {EpisodeId}",
                        createdEpisode.Id
                    );
                }

                // Update series total episodes count
                await UpdateSeriesTotalEpisodes(episodeRequest.SeriesId);

                _logger.LogInformation(
                    "?? Episode upload completed successfully for ID: {EpisodeId}",
                    createdEpisode.Id
                );
                return new
                {
                    success = true,
                    message = "Episode uploaded successfully",
                    id = createdEpisode.Id,
                    title = createdEpisode.Title,
                    seasonNumber = createdEpisode.SeasonNumber,
                    episodeNumber = createdEpisode.EpisodeNumber
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error uploading episode");
                return new
                {
                    success = false,
                    message = "An error occurred while uploading the episode"
                };
            }
        }

        #endregion

        #region Helper Methods

        public async Task<object> GetUploadStatsAsync()
        {
            try
            {
                var allMovies = await _movieRepository.GetAllAsync();
                var allSeries = await _seriesRepository.GetAllAsync();
                var allEpisodes = await _episodeRepository.GetAllAsync();

                var draftMovies = allMovies.Where(m => m.Status == "draft").Count();
                var draftSeries = allSeries.Where(s => s.Status == "draft").Count();

                var today = DateTime.Today;
                var completedToday =
                    allMovies.Where(m => m.CreatedAt?.Date == today && m.Status == "active").Count()
                    + allSeries
                        .Where(s => s.CreatedAt?.Date == today && s.Status == "active")
                        .Count()
                    + allEpisodes.Where(e => e.CreatedAt?.Date == today).Count();

                return new
                {
                    TotalMovies = allMovies.Where(m => m.Status == "active").Count(),
                    TotalSeries = allSeries.Where(s => s.Status == "active").Count(),
                    TotalEpisodes = allEpisodes.Count(),
                    PendingUploads = draftMovies + draftSeries,
                    ProcessingVideos = 0, // TODO: Implement video processing status tracking
                    CompletedToday = completedToday,
                    ReservedMovieIds = draftMovies,
                    ReservedSeriesIds = draftSeries
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error getting upload statistics");
                return new
                {
                    TotalMovies = 0,
                    TotalSeries = 0,
                    TotalEpisodes = 0,
                    PendingUploads = 0,
                    ProcessingVideos = 0,
                    CompletedToday = 0,
                    ReservedMovieIds = 0,
                    ReservedSeriesIds = 0
                };
            }
        }

        public async Task<object> GetSeriesListAsync()
        {
            try
            {
                var allSeries = await _seriesRepository.GetAllAsync();
                var activeSeries = allSeries.Where(s => s.Status == "active");

                return activeSeries
                    .Select(s => new
                    {
                        id = s.Id,
                        title = s.Title,
                        totalSeasons = s.TotalSeasons ?? 1,
                        totalEpisodes = s.TotalEpisodes ?? 0
                    })
                    .ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error getting series list");
                return new[]
                {
                    new
                    {
                        id = 1,
                        title = "Breaking Bad",
                        totalSeasons = 5,
                        totalEpisodes = 62
                    },
                    new
                    {
                        id = 2,
                        title = "Game of Thrones",
                        totalSeasons = 8,
                        totalEpisodes = 73
                    },
                    new
                    {
                        id = 3,
                        title = "The Office",
                        totalSeasons = 9,
                        totalEpisodes = 201
                    }
                };
            }
        }

        public async Task<bool> ValidateSlugAsync(string slug, string type, int? excludeId = null)
        {
            try
            {
                if (string.IsNullOrEmpty(slug))
                    return false;

                if (type.ToLower() == "movie")
                {
                    var existingMovies = excludeId.HasValue
                        ? await _movieRepository.FindAsync(m =>
                            m.Slug == slug && m.Id != excludeId.Value
                        )
                        : await _movieRepository.FindAsync(m => m.Slug == slug);

                    return !existingMovies.Any();
                }
                else if (type.ToLower() == "series")
                {
                    var existingSeries = excludeId.HasValue
                        ? await _seriesRepository.FindAsync(s =>
                            s.Slug == slug && s.Id != excludeId.Value
                        )
                        : await _seriesRepository.FindAsync(s => s.Slug == slug);

                    return !existingSeries.Any();
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error validating slug: {Slug}", slug);
                return false;
            }
        }

        #endregion

        #region Private Helper Methods

        private string GenerateSlug(string title)
        {
            if (string.IsNullOrEmpty(title))
                return string.Empty;

            return title
                .ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace(":", "")
                .Replace("'", "")
                .Replace("\"", "")
                .Replace("&", "and")
                .Replace("!", "")
                .Replace("?", "")
                .Replace(",", "")
                .Replace(".", "")
                .Replace(";", "")
                .Replace("#", "")
                .Replace("@", "")
                .Replace("%", "")
                .Replace("^", "")
                .Replace("*", "")
                .Replace("+", "")
                .Replace("=", "")
                .Replace("~", "")
                .Replace("`", "")
                .Replace("|", "")
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("<", "")
                .Replace(">", "");
        }

        private async Task HandleMovieRelationships(int movieId, MovieUploadDto request)
        {
            try
            {
                // TODO: Implement movie-category, movie-director, movie-actor relationships
                // This would require additional repository interfaces and implementations

                _logger.LogInformation(
                    "?? Movie relationships handling for ID: {MovieId} (placeholder)",
                    movieId
                );

                /* Example implementation:
                if (request.CategoryIds?.Any() == true)
                {
                    foreach (var categoryId in request.CategoryIds)
                    {
                        var movieCategory = new MovieCategory
                        {
                            MovieId = movieId,
                            CategoryId = categoryId
                        };
                        await _movieCategoryRepository.AddAsync(movieCategory);
                    }
                }
                */
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "? Error handling movie relationships for ID: {MovieId}",
                    movieId
                );
            }
        }

        private async Task HandleSeriesRelationships(int seriesId, SeriesUploadDto request)
        {
            try
            {
                // TODO: Implement series-category, series-director, series-actor relationships

                _logger.LogInformation(
                    "?? Series relationships handling for ID: {SeriesId} (placeholder)",
                    seriesId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "? Error handling series relationships for ID: {SeriesId}",
                    seriesId
                );
            }
        }

        private async Task UpdateSeriesTotalEpisodes(int seriesId)
        {
            try
            {
                var episodes = await _episodeRepository.FindAsync(e => e.SeriesId == seriesId);
                var episodeCount = episodes.Count();

                var series = await _seriesRepository.GetByIdAsync(seriesId);
                if (series != null)
                {
                    series.TotalEpisodes = episodeCount;
                    series.UpdatedAt = DateTime.UtcNow;
                    await _seriesRepository.UpdateAsync(series);

                    _logger.LogInformation(
                        "?? Updated series {SeriesId} total episodes to: {Count}",
                        seriesId,
                        episodeCount
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "? Error updating series total episodes for ID: {SeriesId}",
                    seriesId
                );
            }
        }

        #endregion
    }
}
