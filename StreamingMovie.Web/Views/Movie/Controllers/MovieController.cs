using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using System.Security.Claims;

namespace StreamingMovie.Web.Views.Movie.Controllers
{
    public class MovieController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUnifiedMovieService _unifiedMovieService;
        private readonly ICategoryService _categoryService;
        private readonly IMovieService _movieService;
        private readonly IRatingService _ratingService;

        public MovieController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IUnifiedMovieService unifiedMovieService,
            ICategoryService categoryService,
            IMovieService movieService,
            IRatingService ratingService
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _unifiedMovieService = unifiedMovieService;
            _categoryService = categoryService;
            _movieService = movieService;
            _ratingService = ratingService;
        }

        public async Task<IActionResult> Details(string slug, int? page = 1)
        {
            var response = await _unifiedMovieService.GetMovieDetails(slug);
            if (response != null)
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int? userId = null;

                if (int.TryParse(userIdClaim, out int parsedUserId))
                {
                    userId = parsedUserId;
                }

                var pagedRating = await _ratingService.PaginateBySlugAsync(slug, parsedUserId, page ?? 1);
                response.Ratings = pagedRating;

                if (userId != null)
                    response.UserReview = await _ratingService.GetUserReview(parsedUserId, slug);
            }
            return View(response);
        }

        public async Task<IActionResult> Watching(string slug)
        {
            var unifiedMovie = await _unifiedMovieService.Find(x => x.Slug == slug).FirstOrDefaultAsync();

            if (unifiedMovie == null)
            {
                return NotFound("Movie not found");
            }

            if (!unifiedMovie.IsSeries)
            {
                try
                {
                    var movie = await _movieService.GetMovieVideoAsync(unifiedMovie.Id);
                    if (movie == null)
                    {
                        return NotFound("Movie not found");
                    }
                    var movieUpdate = await _movieService.FindOneAsync(m => m.Id == unifiedMovie.Id);
                    if (movieUpdate != null)
                    {
                        movieUpdate.ViewCount += 1;
                        await _movieService.UpdateAsync(movieUpdate);
                    }

                    return View(movie);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound("Movie or video not found");
                }
                catch (Exception ex)
                {
                    // Log the error in a real application
                    return BadRequest($"Error loading movie: {ex.Message}");
                }
            }
            else
            {
                return RedirectToAction("Details", "Movie", new
                {
                    slug = unifiedMovie.Slug
                });
            }

        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string q, string returnUrl = null)
        {
            var filter = new MovieFilterDTO { Keyword = q, Page = 1, };
            var sectionTitle = string.IsNullOrEmpty(q) ? "Search" : $"Search results for '{q}'";

            return await RenderMovieList(filter, sectionTitle, returnUrl);
        }

        [HttpGet("category")]
        public async Task<IActionResult> Category(string slug, string returnUrl = null)
        {
            string sectionTitle;
            if (string.IsNullOrEmpty(slug))
            {
                sectionTitle = "All Movies";
            }
            else
            {
                var category = await _categoryService.FindOneAsync(c => c.Slug == slug);
                if (category == null)
                    return RedirectToAction("Error404", "Home");

                sectionTitle = category.Name;
            }

            var filter = new MovieFilterDTO
            {
                Categories = string.IsNullOrEmpty(slug) ? null : new List<string> { slug },
                Page = 1,
            };

            return await RenderMovieList(filter, sectionTitle, returnUrl);
        }

        private async Task<IActionResult> RenderMovieList(
            MovieFilterDTO filter,
            string? sectionTitle,
            string returnUrl
        )
        {
            var result = await _unifiedMovieService.GetFilteredPagedMovies(filter);

            ViewBag.Filter = filter;
            ViewBag.SectionTitle = sectionTitle;
            ViewData["ReturnUrl"] = returnUrl;
            return View("MovieList", result);
        }

        // This action is used for AJAX requests to filter movies based on the provided criteria.
        [HttpGet("filter")]
        public async Task<IActionResult> Filter(
            [FromQuery] MovieFilterDTO filter,
            string? returnUrl = null,
            string? sectionTitle = "Browse"
        )
        {
            var result = await _unifiedMovieService.GetFilteredPagedMovies(filter);

            ViewBag.Filter = filter;
            ViewBag.SectionTitle = sectionTitle;
            ViewData["ReturnUrl"] = returnUrl;


            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_MovieListPartial", result);
            }
            return View("MovieList", result);
        }
    }
}
