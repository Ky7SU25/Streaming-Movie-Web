using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Web.Views.Movie.Controllers
{
    public class MovieController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUnifiedMovieService _unifiedMovieService;
        private readonly ICategoryService _categoryService;
        private readonly IMovieService _movieService;

        public MovieController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IUnifiedMovieService movieService,
            ICategoryService categoryService,
            IMovieService movieService1
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _unifiedMovieService = movieService;
            _categoryService = categoryService;
            _movieService = movieService1;
        }

        public async Task<IActionResult> Details(string slug)
        {
            var response = await _unifiedMovieService.GetMovieDetails(slug);
            return View(response);
        }

        public async Task<IActionResult> Watching(int id)
        {
            try
            {
                var movie = await _movieService.GetMovieVideoAsync(id);
                if (movie == null)
                {
                    return NotFound("Movie not found");
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

        //[HttpGet("search")]
        //public async Task<IActionResult> Search(string q, string returnUrl = null)
        //{
        //    var filter = new MovieFilterDTO { Keyword = q, Page = 1, };
        //    var sectionTitle = string.IsNullOrEmpty(q) ? "Search" : $"Search results for '{q}'";

        //    return await RenderMovieList(filter, sectionTitle, returnUrl);
        //}

        [HttpGet("search")]
        public async Task<IActionResult> Search(string q)
        {
           var result = await _unifiedMovieService.GetAISearchPagedMovies(q);
            var filter = new MovieFilterDTO
            {
                Keyword = q,
                Page = 1,
            };
            var sectionTitle = string.IsNullOrEmpty(q) ? "Search" : $"Search results for '{q}'";
            ViewBag.Filter = filter;
            ViewBag.SectionTitle = sectionTitle;
            return View("MovieList", result);
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

        private float CosineSimilarity(float[] vec1, float[] vec2)
        {
            if (vec1.Length != vec2.Length || vec1.Length == 0)
                return 0;

            float dot = 0, mag1 = 0, mag2 = 0;
            for (int i = 0; i < vec1.Length; i++)
            {
                dot += vec1[i] * vec2[i];
                mag1 += vec1[i] * vec1[i];
                mag2 += vec2[i] * vec2[i];
            }
            return dot / ((float)Math.Sqrt(mag1) * (float)Math.Sqrt(mag2));
        }
    }
}
