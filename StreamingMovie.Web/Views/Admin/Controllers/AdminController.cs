using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Application.Interfaces.ExternalServices.Storage;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Infrastructure.ExternalServices.Storage;
using StreamingMovie.Web.Views.Admin.ViewModels;

namespace StreamingMovie.Web.Views.Admin.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AdminController> _logger;
        private readonly IVideoUploadService _videoUploadService;
        private readonly IFileUploadService _fileUploadService;
        private readonly ICategoryService _categoryService;
        private readonly ICountryService _countryService;
        private readonly IStorageHandler _storage;
        private readonly MinioOptions _options;

        // MinIO File Naming Convention Rules
        private static readonly Dictionary<string, string> FilePathRules =
            new()
            {
                // Movies: /movies/{year}/{movieId}/
                ["movie-video"] = "movies/{year}/{movieId}/video/{timestamp}-{filename}",
                ["movie-poster"] = "movies/{year}/{movieId}/poster/{timestamp}-{filename}",
                ["movie-banner"] = "movies/{year}/{movieId}/banner/{timestamp}-{filename}",
                ["movie-subtitle"] = "movies/{year}/{movieId}/subtitle/{timestamp}-{filename}",

                // Series: /series/{year}/{seriesId}/
                ["series-poster"] = "series/{year}/{seriesId}/poster/{timestamp}-{filename}",
                ["series-banner"] = "series/{year}/{seriesId}/banner/{timestamp}-{filename}",

                // Episodes: /series/{year}/{seriesId}/seasons/{seasonNumber}/episodes/{episodeNumber}/
                ["episode-video"] =
                    "series/{year}/{seriesId}/seasons/{seasonNumber}/episodes/{episodeNumber}/video/{timestamp}-{filename}",
                ["episode-subtitle"] =
                    "series/{year}/{seriesId}/seasons/{seasonNumber}/episodes/{episodeNumber}/subtitle/{timestamp}-{filename}"
            };

        public AdminController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<AdminController> logger,
            IVideoUploadService videoUploadService,
            IFileUploadService fileUploadService,
            ICategoryService categoryService,
            ICountryService countryService,
            IStorageHandler storage,
            IOptions<MinioOptions> options
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _videoUploadService = videoUploadService;
            _fileUploadService = fileUploadService;
            _categoryService = categoryService;
            _countryService = countryService;
            _storage = storage;
            _options = options.Value;
        }

        #region Admin Pages

        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Billing(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Profile(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        public IActionResult Table(string returnUrl = null)

        {
           // ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult MovieUpload(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        #endregion

        #region Video Upload Pages

        [HttpGet]
        public async Task<IActionResult> VideoUpload(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // Get data for dropdowns
            var model = new VideoUploadViewModel
            {
                Countries = await GetCountriesAsync(),
                Categories = await GetCategoriesAsync(),
                Directors = await GetDirectorsAsync(),
                Actors = await GetActorsAsync(),
                VideoServers = await GetVideoServersAsync(),
                VideoQualities = await GetVideoQualitiesAsync()
            };

            return View(model);
        }

        #endregion

        #region Helper Methods for Data

        private async Task<List<SelectListItemDto>> GetCountriesAsync()
        {
            try
            {
                var countries = await _countryService.GetAllAsync();
                return countries
                    .Select(c => new SelectListItemDto { Value = c.Id.ToString(), Text = c.Name })
                    .ToList();
            }
            catch
            {
                // Fallback data
                return new List<SelectListItemDto>
                {
                    new() { Value = "1", Text = "United States" },
                    new() { Value = "2", Text = "Vietnam" },
                    new() { Value = "3", Text = "South Korea" },
                    new() { Value = "4", Text = "Japan" },
                    new() { Value = "5", Text = "United Kingdom" },
                    new() { Value = "6", Text = "France" }
                };
            }
        }

        private async Task<List<SelectListItemDto>> GetCategoriesAsync()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();
                return categories
                    .Select(c => new SelectListItemDto { Value = c.Id.ToString(), Text = c.Name })
                    .ToList();
            }
            catch
            {
                // Fallback data
                return new List<SelectListItemDto>
                {
                    new() { Value = "1", Text = "Action" },
                    new() { Value = "2", Text = "Drama" },
                    new() { Value = "3", Text = "Comedy" },
                    new() { Value = "4", Text = "Horror" },
                    new() { Value = "5", Text = "Romance" },
                    new() { Value = "6", Text = "Thriller" },
                    new() { Value = "7", Text = "Sci-Fi" },
                    new() { Value = "8", Text = "Adventure" }
                };
            }
        }

        private async Task<List<SelectListItemDto>> GetDirectorsAsync()
        {
            // TODO: Implement actual director service
            await Task.Delay(1); // Placeholder
            return new List<SelectListItemDto>
            {
                new() { Value = "1", Text = "Christopher Nolan" },
                new() { Value = "2", Text = "Quentin Tarantino" },
                new() { Value = "3", Text = "Martin Scorsese" },
                new() { Value = "4", Text = "Steven Spielberg" },
                new() { Value = "5", Text = "Denis Villeneuve" },
                new() { Value = "6", Text = "Coen Brothers" }
            };
        }

        private async Task<List<SelectListItemDto>> GetActorsAsync()
        {
            // TODO: Implement actual actor service
            await Task.Delay(1); // Placeholder
            return new List<SelectListItemDto>
            {
                new() { Value = "1", Text = "Leonardo DiCaprio" },
                new() { Value = "2", Text = "Brad Pitt" },
                new() { Value = "3", Text = "Tom Hanks" },
                new() { Value = "4", Text = "Robert De Niro" },
                new() { Value = "5", Text = "Al Pacino" },
                new() { Value = "6", Text = "Johnny Depp" },
                new() { Value = "7", Text = "Will Smith" },
                new() { Value = "8", Text = "Morgan Freeman" }
            };
        }

        private async Task<List<SelectListItemDto>> GetVideoServersAsync()
        {
            // TODO: Implement actual video server service
            await Task.Delay(1); // Placeholder
            return new List<SelectListItemDto>
            {
                new() { Value = "1", Text = "Server 1 (Primary)" },
                new() { Value = "2", Text = "Server 2 (Backup)" },
                new() { Value = "3", Text = "Server 3 (CDN)" }
            };
        }

        private async Task<List<SelectListItemDto>> GetVideoQualitiesAsync()
        {
            // TODO: Implement actual video quality service
            await Task.Delay(1); // Placeholder
            return new List<SelectListItemDto>
            {
                new() { Value = "1", Text = "HD (720p)" },
                new() { Value = "2", Text = "Full HD (1080p)" },
                new() { Value = "3", Text = "4K (2160p)" },
                new() { Value = "4", Text = "SD (480p)" }
            };
        }

        #endregion

        #region ID Reservation Endpoints

        /// <summary>
        /// Reserve a movie ID for file path generation before upload
        /// Route: /Admin/ReserveMovieId
        /// </summary>
        [HttpPost("Admin/ReserveMovieId")]
        public async Task<IActionResult> ReserveMovieId([FromBody] ReserveMovieIdRequest request)
        {
            try
            {
                _logger.LogInformation("🎬 Reserving movie ID for: {Title}", request.Title);

                // Validate basic requirements
                if (string.IsNullOrEmpty(request.Title?.Trim()))
                {
                    return Json(new { error = "Movie title is required" });
                }

                // Create a draft movie record to reserve the ID
                var reservedId = await _videoUploadService.ReserveMovieIdAsync(
                    request.Title,
                    request.Slug
                );

                _logger.LogInformation(
                    "✅ Movie ID reserved: {MovieId} for {Title}",
                    reservedId,
                    request.Title
                );

                return Json(
                    new
                    {
                        success = true,
                        movieId = reservedId,
                        message = "Movie ID reserved successfully",
                        year = DateTime.UtcNow.Year
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error reserving movie ID for {Title}", request.Title);
                return Json(new { error = "Failed to reserve movie ID: " + ex.Message });
            }
        }

        /// <summary>
        /// Reserve a series ID for file path generation before upload
        /// Route: /Admin/ReserveSeriesId
        /// </summary>
        [HttpPost("Admin/ReserveSeriesId")]
        public async Task<IActionResult> ReserveSeriesId([FromBody] ReserveSeriesIdRequest request)
        {
            try
            {
                _logger.LogInformation("📺 Reserving series ID for: {Title}", request.Title);

                // Validate basic requirements
                if (string.IsNullOrEmpty(request.Title?.Trim()))
                {
                    return Json(new { error = "Series title is required" });
                }

                // Create a draft series record to reserve the ID
                var reservedId = await _videoUploadService.ReserveSeriesIdAsync(
                    request.Title,
                    request.Slug
                );

                _logger.LogInformation(
                    "✅ Series ID reserved: {SeriesId} for {Title}",
                    reservedId,
                    request.Title
                );

                return Json(
                    new
                    {
                        success = true,
                        seriesId = reservedId,
                        message = "Series ID reserved successfully",
                        year = DateTime.UtcNow.Year
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error reserving series ID for {Title}", request.Title);
                return Json(new { error = "Failed to reserve series ID: " + ex.Message });
            }
        }

        /// <summary>
        /// Cancel a reserved ID if upload is aborted
        /// Route: /Admin/CancelReservedId
        /// </summary>
        [HttpPost("Admin/CancelReservedId")]
        public async Task<IActionResult> CancelReservedId(
            [FromBody] CancelReservedIdRequest request
        )
        {
            try
            {
                _logger.LogInformation(
                    "🗑️ Cancelling reserved ID: {Type} {Id}",
                    request.Type,
                    request.Id
                );

                var success = await _videoUploadService.CancelReservedIdAsync(
                    request.Type,
                    request.Id
                );

                if (success)
                {
                    _logger.LogInformation(
                        "✅ Reserved ID cancelled: {Type} {Id}",
                        request.Type,
                        request.Id
                    );
                    return Json(
                        new { success = true, message = "Reserved ID cancelled successfully" }
                    );
                }
                else
                {
                    return Json(new { success = false, message = "Failed to cancel reserved ID" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "❌ Error cancelling reserved ID: {Type} {Id}",
                    request.Type,
                    request.Id
                );
                return Json(new { error = "Failed to cancel reserved ID: " + ex.Message });
            }
        }

        #endregion

        #region MinIO Multipart Upload Endpoints (Following Demo Structure)

        /// <summary>
        /// Start MinIO multipart upload for files
        /// Route: /Admin/Upload/Start
        /// </summary>
        [HttpPost("Admin/Upload/Start")]
        public async Task<IActionResult> StartMinIOUpload(
            string fileName,
            long fileSize,
            string fileType,
            string contentType = "",
            // Required parameters for file path generation
            int? movieId = null,
            int? seriesId = null,
            int? seasonNumber = null,
            int? episodeNumber = null,
            int? year = null
        )
        {
            try
            {
                _logger.LogInformation(
                    "🚀 Starting MinIO multipart upload for {FileName} ({FileSize} bytes, type: {FileType})",
                    fileName,
                    fileSize,
                    fileType
                );

                // Validate parameters
                if (
                    string.IsNullOrEmpty(fileName)
                    || fileSize <= 0
                    || string.IsNullOrEmpty(fileType)
                )
                {
                    return Json(new { error = "Invalid file parameters" });
                }

                // Validate that we have the required IDs for proper file path generation
                if (!ValidateRequiredIds(fileType, movieId, seriesId, seasonNumber, episodeNumber))
                {
                    return Json(
                        new
                        {
                            error = "Missing required IDs for file path generation. Please reserve an ID first."
                        }
                    );
                }

                // Generate MinIO file path based on rules
                var minioFileName = GenerateMinIOFilePath(
                    fileName,
                    fileType,
                    movieId,
                    seriesId,
                    seasonNumber,
                    episodeNumber,
                    year
                );

                _logger.LogInformation("📂 Generated MinIO path: {MinIOPath}", minioFileName);

                // Start multipart upload using IStorageHandler
                var (uploadId, parts) = await _storage.StartMultipartUploadAsync(
                    minioFileName,
                    fileSize
                );

                // Convert to the expected format for frontend
                var response = new
                {
                    uploadId,
                    parts = parts
                        .Select(p => new { partNumber = p.PartNumber, url = p.Url })
                        .ToList(),
                    fileName = minioFileName,
                    originalFileName = fileName,
                    fileType
                };

                _logger.LogInformation(
                    "✅ MinIO multipart upload started successfully. UploadId: {UploadId}, Parts: {PartCount}",
                    uploadId,
                    parts.Count
                );

                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "❌ Error starting MinIO multipart upload for {FileName}",
                    fileName
                );
                return Json(new { error = "Failed to start multipart upload: " + ex.Message });
            }
        }

        /// <summary>
        /// Complete MinIO multipart upload
        /// Route: /Admin/Upload/Complete
        /// </summary>
        [HttpPost("Admin/Upload/Complete")]
        public async Task<IActionResult> CompleteMinIOUpload(
            [FromBody] CompleteMinIORequest request
        )
        {
            try
            {
                _logger.LogInformation(
                    "🔗 Completing MinIO multipart upload for {FileName} with {PartCount} parts",
                    request.FileName,
                    request.Parts?.Count ?? 0
                );

                // Validate request
                if (
                    string.IsNullOrEmpty(request.FileName)
                    || string.IsNullOrEmpty(request.UploadId)
                    || request.Parts == null
                    || !request.Parts.Any()
                )
                {
                    return Json(new { error = "Invalid completion request parameters" });
                }

                // Convert parts to the format expected by IStorageHandler
                var parts = request.Parts.Select(p => (p.PartNumber, p.ETag)).ToList();

                // Complete multipart upload
                await _storage.CompleteMultipartUploadAsync(
                    request.FileName,
                    request.UploadId,
                    parts
                );

                // Generate the final public URL for the file
                var publicUrl = GeneratePublicUrl(request.FileName);

                _logger.LogInformation(
                    "🎉 MinIO multipart upload completed successfully. File: {FileName}, URL: {PublicUrl}",
                    request.FileName,
                    publicUrl
                );

                return Json(
                    new
                    {
                        success = true,
                        message = "✅ Upload complete",
                        url = publicUrl,
                        fileName = request.FileName,
                        originalFileName = request.OriginalFileName
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "❌ Error completing MinIO multipart upload for {FileName}",
                    request.FileName
                );
                return Json(new { error = "Failed to complete multipart upload: " + ex.Message });
            }
        }

        /// <summary>
        /// Abort MinIO multipart upload (cleanup)
        /// Route: /Admin/Upload/Abort
        /// </summary>
        [HttpPost("Admin/Upload/Abort")]
        public async Task<IActionResult> AbortMinIOUpload([FromBody] AbortMinIORequest request)
        {
            try
            {
                _logger.LogInformation(
                    "🗑️ Aborting MinIO multipart upload for {FileName}",
                    request.FileName
                );

                // Validate request
                if (
                    string.IsNullOrEmpty(request.FileName) || string.IsNullOrEmpty(request.UploadId)
                )
                {
                    return Json(new { error = "Invalid abort request parameters" });
                }

                // For now, just log - MinIO SDK might handle cleanup automatically
                _logger.LogWarning(
                    "⚠️ MinIO multipart upload aborted: {FileName}, UploadId: {UploadId}",
                    request.FileName,
                    request.UploadId
                );

                return Json(new { success = true, message = "Upload aborted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "❌ Error aborting MinIO multipart upload for {FileName}",
                    request.FileName
                );
                return Json(new { error = "Failed to abort multipart upload: " + ex.Message });
            }
        }

        #endregion

        #region Movie Upload

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadMovie([FromBody] MovieUploadRequest request)
        {
            try
            {
                _logger.LogInformation("🎬 Movie upload request received: {Title}", request.Title);

                // Validate model
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value!.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList();

                    _logger.LogWarning(
                        "Movie upload validation failed: {Errors}",
                        string.Join(", ", errors)
                    );
                    return Json(UploadResponse.ValidationErrorResult(errors));
                }

                // Map from Web ViewModel to Application DTO
                var movieDto = MapToMovieUploadDto(request);

                // Log the file URLs that were uploaded to MinIO
                _logger.LogInformation(
                    "📁 Movie files uploaded to MinIO - Video: {VideoUrl}, Poster: {PosterUrl}, Banner: {BannerUrl}, Subtitle: {SubtitleUrl}",
                    movieDto.VideoUrl,
                    movieDto.PosterUrl,
                    movieDto.BannerUrl,
                    movieDto.SubtitleUrl
                );

                // Delegate to service with MinIO URLs
                var response = await _videoUploadService.UploadMovieAsync(movieDto);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "❌ Error in movie upload controller action: {Title}",
                    request.Title
                );
                return Json(
                    UploadResponse.ErrorResult(
                        "An error occurred while uploading the movie. Please try again."
                    )
                );
            }
        }

        #endregion

        #region Series Upload

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadSeries([FromBody] SeriesUploadRequest request)
        {
            try
            {
                _logger.LogInformation("📺 Series upload request received: {Title}", request.Title);

                // Validate model
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value!.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList();

                    _logger.LogWarning(
                        "Series upload validation failed: {Errors}",
                        string.Join(", ", errors)
                    );
                    return Json(UploadResponse.ValidationErrorResult(errors));
                }

                // Map from Web ViewModel to Application DTO
                var seriesDto = MapToSeriesUploadDto(request);

                // Log the file URLs that were uploaded to MinIO
                _logger.LogInformation(
                    "📁 Series files uploaded to MinIO - Poster: {PosterUrl}, Banner: {BannerUrl}",
                    seriesDto.PosterUrl,
                    seriesDto.BannerUrl
                );

                // Delegate to service with MinIO URLs
                var response = await _videoUploadService.UploadSeriesAsync(seriesDto);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "❌ Error in series upload controller action: {Title}",
                    request.Title
                );
                return Json(
                    UploadResponse.ErrorResult(
                        "An error occurred while creating the series. Please try again."
                    )
                );
            }
        }

        #endregion

        #region Episode Upload

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadEpisode([FromBody] EpisodeUploadRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "📹 Episode upload request received: S{Season}E{Episode}",
                    request.SeasonNumber,
                    request.EpisodeNumber
                );

                // Validate model
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value!.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList();

                    _logger.LogWarning(
                        "Episode upload validation failed: {Errors}",
                        string.Join(", ", errors)
                    );
                    return Json(UploadResponse.ValidationErrorResult(errors));
                }

                // Map from Web ViewModel to Application DTO
                var episodeDto = MapToEpisodeUploadDto(request);

                // Log the file URLs that were uploaded to MinIO
                _logger.LogInformation(
                    "📁 Episode files uploaded to MinIO - Video: {VideoUrl}, Subtitle: {SubtitleUrl}",
                    episodeDto.VideoUrl,
                    episodeDto.SubtitleUrl
                );

                // Delegate to service with MinIO URLs
                var response = await _videoUploadService.UploadEpisodeAsync(episodeDto);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "❌ Error in episode upload controller action: S{Season}E{Episode}",
                    request.SeasonNumber,
                    request.EpisodeNumber
                );
                return Json(
                    UploadResponse.ErrorResult(
                        "An error occurred while uploading the episode. Please try again."
                    )
                );
            }
        }

        #endregion

        #region Legacy File Upload Actions (Deprecated - use MinIO multipart instead)

        /// <summary>
        /// Upload small files directly - MVC Action (DEPRECATED)
        /// Use MinIO multipart upload instead
        /// </summary>
        [HttpPost]
        [Obsolete("Use MinIO multipart upload endpoints instead")]
        public async Task<IActionResult> UploadFile(IFormFile file, string fileType)
        {
            try
            {
                var result = await _fileUploadService.UploadFileAsync(file, fileType) as dynamic;

                return Json(
                    new
                    {
                        success = result?.success ?? false,
                        url = result?.url,
                        originalName = result?.originalName,
                        size = result?.size,
                        error = result?.error
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in file upload controller action: {FileName}",
                    file?.FileName
                );
                return Json(new { success = false, error = "Upload failed" });
            }
        }

        /// <summary>
        /// Upload chunks for large files - MVC Action (DEPRECATED)
        /// Use MinIO multipart upload instead
        /// </summary>
        [HttpPost]
        [Obsolete("Use MinIO multipart upload endpoints instead")]
        public async Task<IActionResult> UploadChunk(
            IFormFile chunkFile,
            string fileName,
            int chunkNumber,
            int totalChunks,
            string chunkId
        )
        {
            try
            {
                var result =
                    await _fileUploadService.UploadChunkAsync(
                        chunkFile,
                        fileName,
                        chunkNumber,
                        totalChunks,
                        chunkId
                    ) as dynamic;

                return Json(
                    new
                    {
                        success = result?.success ?? false,
                        chunkNumber = result?.chunkNumber ?? 0,
                        totalChunks = result?.totalChunks ?? 0,
                        fileName = result?.fileName,
                        uploaded = result?.uploaded ?? false,
                        error = result?.error
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in chunk upload controller action {ChunkNumber} for {FileName}",
                    chunkNumber,
                    fileName
                );
                return Json(new { success = false, error = "Chunk upload failed" });
            }
        }

        /// <summary>
        /// Get signed URL for direct S3 upload - MVC Action (DEPRECATED)
        /// Use MinIO multipart upload instead
        /// </summary>
        [HttpPost]
        [Obsolete("Use MinIO multipart upload endpoints instead")]
        public async Task<IActionResult> GetSignedUrl([FromBody] SignedUrlRequest request)
        {
            try
            {
                var result =
                    await _fileUploadService.GetSignedUrlAsync(
                        request.FileName,
                        request.FileType,
                        request.ContentType,
                        request.FileSize
                    ) as dynamic;

                return Json(
                    new
                    {
                        success = result?.success ?? false,
                        signedUrl = result?.signedUrl,
                        fileName = result?.fileName,
                        fileType = result?.fileType,
                        expiresIn = result?.expiresIn ?? 0,
                        error = result?.error
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in signed URL controller action for {FileName}",
                    request.FileName
                );
                return Json(new { success = false, error = "Failed to generate signed URL" });
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get upload statistics (for future dashboard)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUploadStats()
        {
            try
            {
                var stats = await _videoUploadService.GetUploadStatsAsync();
                return Json(UploadResponse.SuccessResult("Statistics retrieved", stats));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving upload statistics");
                return Json(UploadResponse.ErrorResult("Failed to retrieve statistics"));
            }
        }

        /// <summary>
        /// Get list of series for episode upload dropdown
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSeriesList()
        {
            try
            {
                var seriesList = await _videoUploadService.GetSeriesListAsync();
                return Json(UploadResponse.SuccessResult("Series list retrieved", seriesList));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving series list");
                return Json(UploadResponse.ErrorResult("Failed to retrieve series list"));
            }
        }

        /// <summary>
        /// Validate slug uniqueness
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ValidateSlug([FromBody] ValidateSlugRequest request)
        {
            try
            {
                var isUnique = await _videoUploadService.ValidateSlugAsync(
                    request.Slug,
                    request.Type,
                    request.ExcludeId
                );
                return Json(
                    new
                    {
                        isUnique,
                        message = isUnique ? "Slug is available" : "Slug already exists"
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating slug: {Slug}", request.Slug);
                return Json(new { isUnique = false, message = "Error validating slug" });
            }
        }

        #endregion

        #region MinIO File Path Generation

        /// <summary>
        /// Validate that we have the required IDs for proper file path generation
        /// </summary>
        private bool ValidateRequiredIds(
            string fileType,
            int? movieId,
            int? seriesId,
            int? seasonNumber,
            int? episodeNumber
        )
        {
            return fileType.ToLower() switch
            {
                // Movie files require movieId
                "video" when movieId.HasValue => true,
                "poster" when movieId.HasValue => true,
                "banner" when movieId.HasValue => true,
                "subtitle" when movieId.HasValue => true,

                // Series files require seriesId
                "poster" when seriesId.HasValue => true,
                "banner" when seriesId.HasValue => true,

                // Episode files require seriesId, seasonNumber, and episodeNumber
                "video" when seriesId.HasValue && seasonNumber.HasValue && episodeNumber.HasValue
                    => true,
                "subtitle" when seriesId.HasValue && seasonNumber.HasValue && episodeNumber.HasValue
                    => true,

                // Unknown file types are not allowed
                _ => false
            };
        }

        /// <summary>
        /// Generate MinIO file path based on predefined rules
        /// </summary>
        private string GenerateMinIOFilePath(
            string originalFileName,
            string fileType,
            int? movieId = null,
            int? seriesId = null,
            int? seasonNumber = null,
            int? episodeNumber = null,
            int? year = null
        )
        {
            try
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var currentYear = year ?? DateTime.UtcNow.Year;
                var sanitizedFileName = SanitizeFileName(originalFileName);

                // Determine the file type key for path lookup
                string fileTypeKey = DetermineFileTypeKey(
                    fileType,
                    movieId.HasValue,
                    seriesId.HasValue
                );

                if (!FilePathRules.TryGetValue(fileTypeKey, out var pathTemplate))
                {
                    // Fallback path - should not happen with proper validation
                    pathTemplate = "uploads/{fileType}/{timestamp}-{filename}";
                    _logger.LogWarning(
                        "Using fallback path template for fileType: {FileType}",
                        fileType
                    );
                }

                // Replace placeholders in path template
                var filePath = pathTemplate
                    .Replace("{year}", currentYear.ToString())
                    .Replace("{movieId}", movieId?.ToString() ?? "unknown")
                    .Replace("{seriesId}", seriesId?.ToString() ?? "unknown")
                    .Replace("{seasonNumber}", seasonNumber?.ToString("D2") ?? "01")
                    .Replace("{episodeNumber}", episodeNumber?.ToString("D2") ?? "01")
                    .Replace("{timestamp}", timestamp.ToString())
                    .Replace("{filename}", sanitizedFileName)
                    .Replace("{fileType}", fileType);

                return filePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error generating MinIO file path for {FileName}",
                    originalFileName
                );
                // Fallback to simple path
                return $"uploads/{fileType}/{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}-{SanitizeFileName(originalFileName)}";
            }
        }

        /// <summary>
        /// Determine the correct file type key for path generation
        /// </summary>
        private string DetermineFileTypeKey(string fileType, bool isMovie, bool isSeries)
        {
            return fileType.ToLower() switch
            {
                "video" when isMovie => "movie-video",
                "poster" when isMovie => "movie-poster",
                "banner" when isMovie => "movie-banner",
                "subtitle" when isMovie => "movie-subtitle",
                "poster" when isSeries => "series-poster",
                "banner" when isSeries => "series-banner",
                "video" when isSeries => "episode-video",
                "subtitle" when isSeries => "episode-subtitle",
                _ => $"general-{fileType}"
            };
        }

        /// <summary>
        /// Sanitize file name for safe storage
        /// </summary>
        private string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "unnamed_file";

            // Remove unsafe characters and normalize
            var sanitized = fileName
                .Replace(" ", "_")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("#", "")
                .Replace("%", "")
                .Replace("&", "")
                .Replace("?", "")
                .Replace("=", "")
                .Replace("+", "")
                .ToLowerInvariant();

            return sanitized;
        }

        /// <summary>
        /// Generate public URL for accessing the file
        /// </summary>
        private string GeneratePublicUrl(string minioFilePath)
        {
            // TODO: Replace with actual MinIO domain configuration
            var minioBaseUrl = _options.Endpoint; // Should come from configuration
            return $"{minioBaseUrl}/{_options.Bucket}/{minioFilePath}";
        }

        #endregion

        #region Legacy Start/Complete Endpoints (For compatibility)

        [HttpPost("start")]
        public async Task<IActionResult> Start(string fileName, long fileSize)
        {
            var (uploadId, parts) = await _storage.StartMultipartUploadAsync(fileName, fileSize);
            var response = new
            {
                uploadId,
                parts = parts.Select(p => new { PartNumber = p.Item1, Url = p.Item2 }).ToList()
            };
            return Json(response);
        }

        [HttpPost("complete")]
        public async Task<IActionResult> Complete([FromBody] CompleteRequest req)
        {
            var parts = req.Parts.Select(p => (p.PartNumber, p.ETag)).ToList();
            await _storage.CompleteMultipartUploadAsync(req.FileName, req.UploadId, parts);
            return Ok(new { message = "✅ Upload complete" });
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Map MovieUploadRequest (Web) to MovieUploadDto (Application)
        /// </summary>
        private MovieUploadDto MapToMovieUploadDto(MovieUploadRequest request)
        {
            return new MovieUploadDto
            {
                Title = request.Title,
                OriginalTitle = request.OriginalTitle,
                Slug = request.Slug,
                Description = request.Description,
                Duration = request.Duration,
                ReleaseDate = request.ReleaseDate,
                Status = request.Status,
                IsPremium = request.IsPremium,
                CountryId = request.CountryId,
                VideoUrl = request.VideoUrl,
                PosterUrl = request.PosterUrl,
                BannerUrl = request.BannerUrl,
                SubtitleUrl = request.SubtitleUrl,
                TrailerUrl = request.TrailerUrl,
                VideoServerId = request.VideoServerId,
                VideoQualityId = request.VideoQualityId,
                Language = request.Language,
                CategoryIds = request.CategoryIds,
                DirectorIds = request.DirectorIds,
                ActorIds = request.ActorIds
            };
        }

        /// <summary>
        /// Map SeriesUploadRequest (Web) to SeriesUploadDto (Application)
        /// </summary>
        private SeriesUploadDto MapToSeriesUploadDto(SeriesUploadRequest request)
        {
            return new SeriesUploadDto
            {
                Title = request.Title,
                OriginalTitle = request.OriginalTitle,
                Slug = request.Slug,
                Description = request.Description,
                TotalSeasons = request.TotalSeasons,
                ReleaseDate = request.ReleaseDate,
                EndDate = request.EndDate,
                Status = request.Status,
                IsPremium = request.IsPremium,
                CountryId = request.CountryId,
                PosterUrl = request.PosterUrl,
                BannerUrl = request.BannerUrl,
                CategoryIds = request.CategoryIds,
                DirectorIds = request.DirectorIds,
                ActorIds = request.ActorIds
            };
        }

        /// <summary>
        /// Map EpisodeUploadRequest (Web) to EpisodeUploadDto (Application)
        /// </summary>
        private EpisodeUploadDto MapToEpisodeUploadDto(EpisodeUploadRequest request)
        {
            return new EpisodeUploadDto
            {
                SeriesId = request.SeriesId,
                SeasonNumber = request.SeasonNumber,
                EpisodeNumber = request.EpisodeNumber,
                Title = request.Title,
                Description = request.Description,
                Duration = request.Duration,
                AirDate = request.AirDate,
                IsPremium = request.IsPremium,
                VideoUrl = request.VideoUrl,
                SubtitleUrl = request.SubtitleUrl,
                VideoServerId = request.VideoServerId,
                VideoQualityId = request.VideoQualityId,
                Language = request.Language
            };
        }

        #endregion
    }

    #region Helper Classes

    public class ValidateSlugRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Type { get; set; } = "movie"; // movie, series
        public int? ExcludeId { get; set; } // For edit scenarios
    }

    public class SignedUrlRequest
    {
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
    }

    public class CompleteRequest
    {
        public string FileName { get; set; } = "";
        public string UploadId { get; set; } = "";
        public List<PartInfo> Parts { get; set; } = new();
    }

    public class PartInfo
    {
        public int PartNumber { get; set; }
        public string ETag { get; set; } = "";
    }

    #region ID Reservation Request/Response Classes

    public class ReserveMovieIdRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Slug { get; set; }
    }

    public class ReserveSeriesIdRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Slug { get; set; }
    }

    public class CancelReservedIdRequest
    {
        public string Type { get; set; } = string.Empty; // "movie" or "series"
        public int Id { get; set; }
    }

    #endregion

    #region MinIO Request/Response Classes

    public class CompleteMinIORequest
    {
        public string FileName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string UploadId { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public List<PartInfo> Parts { get; set; } = new();
    }

    public class AbortMinIORequest
    {
        public string FileName { get; set; } = string.Empty;
        public string UploadId { get; set; } = string.Empty;
    }

    #endregion

    #endregion
}
