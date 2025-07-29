using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using System.Security.Claims;

namespace StreamingMovie.Web.Views.Movie.Controllers
{
    public class RatingController : Controller
    {
        private readonly IRatingService _ratingService;
        private readonly IMovieService _movieService;
        private readonly ISeriesService _seriesService;

        public RatingController(IRatingService ratingService, IMovieService movieService, ISeriesService seriesService)
        {
            _ratingService = ratingService;
            _movieService = movieService;
            _seriesService = seriesService;
        }

        [HttpGet]
        public async Task<IActionResult> LoadRatings(string slug, int page = 1)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? userId = null;

            if (int.TryParse(userIdClaim, out int parsedUserId))
            {
                userId = parsedUserId;
            }

            var pagedRating = await _ratingService.PaginateBySlugAsync(slug, parsedUserId, page);
            var model = Tuple.Create(pagedRating, slug);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Movie/_RatingPartial.cshtml", model);
            }
            return RedirectToAction("Details", "Movie", new { slug = slug, page = page });
        }

        // POST: /Rating/Create
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(RatingRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
                model.UserId = int.Parse(userId);
            else
                return Unauthorized("User is not authenticated.");

            var result = await _ratingService.AddAsync(model);
            if (result == null)
            {
                TempData["error"] = "Failed to create review.";
            }
            else
            {
                await UpdateRating(result);
                TempData["success"] = "Review created.";
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // POST: /Rating/Update
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(RatingRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model == null || model.Id == null)
            {
                return BadRequest("Invalid rating data.");
            }

            var rating = await _ratingService.GetByIdAsync(model.Id.Value);

            if (rating == null)
            {
                TempData["warning"] = "You can only edit your own reviews.";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                if (rating.RatingValue == model.RatingValue &&
                    (rating.Review == null || rating.Review.Equals(model.Review)))
                {
                    TempData["info"] = "No changes detected in the review.";
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                var result = await _ratingService.UpdateAsync(model);

                if (result == null)
                {
                    TempData["error"] = "Failed to edit review.";
                }
                else
                {
                    await UpdateRating(result);
                    TempData["success"] = "Review edited.";
                }
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

        // POST: /Rating/Delete
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int ratingId)
        {
            var isDeleted = await _ratingService.DeleteAsync(ratingId);
            if (!isDeleted)
            {
                TempData["error"] = "Failed to delete review.";
            }
            else
            {
                TempData["success"] = "Review deleted.";
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        private async Task<bool> UpdateRating(Rating rating)
        {
            if (rating.SeriesId != null)
            {
                var series = await _seriesService.GetByIdAsync(rating.SeriesId.Value);
                if (series != null)
                {
                    series.OurRating = (float) await _ratingService.CalculateSeriesRating(series.Id);
                    return await _seriesService.UpdateAsync(series) != null;
                }
            }
            if (rating.MovieId != null)
            {
                var movie = await _movieService.GetByIdAsync(rating.MovieId.Value);
                if (movie != null)
                {
                    movie.OurRating = (float) await _ratingService.CalculateMovieRating(movie.Id);
                    return await _movieService.UpdateAsync(movie) != null;
                }
            }
            return false;
        }
    }
}

