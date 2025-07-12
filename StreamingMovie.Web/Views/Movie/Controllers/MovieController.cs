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
        private readonly IMovieService _movieService;
        private readonly ICategoryService _categoryService;

        public MovieController(SignInManager<User> signInManager, UserManager<User> userManager, IMovieService movieService, ICategoryService categoryService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _movieService = movieService;
            _categoryService = categoryService;
        }

        public IActionResult Details(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        public IActionResult Watching(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string q, string returnUrl = null)
        {
            var filter = new MovieFilterDTO
            {
                Keyword = q,
                Page = 1,
                PageSize = 3
            };
            var sectionTitle = string.IsNullOrEmpty(q)
                ? "Search"
                : $"Search results for '{q}'";

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
                var category = await _categoryService.GetBySlugAsync(slug);
                if (category == null)
                    return RedirectToAction("Error404", "Home");

                sectionTitle = category.Name;
            }

            var filter = new MovieFilterDTO
            {
                Categories = string.IsNullOrEmpty(slug) ? null : new List<string> { slug },
                Page = 1,
                PageSize = 3
            };

            return await RenderMovieList(filter, sectionTitle, returnUrl);
        }

        private async Task<IActionResult> RenderMovieList(MovieFilterDTO filter, string? sectionTitle, string returnUrl)
        {
            var result = await _movieService.GetPaginatedMovies(filter);

            ViewBag.Filter = filter;
            ViewBag.SectionTitle = sectionTitle;
            ViewData["ReturnUrl"] = returnUrl;
            return View("MovieList", result);
        }

        // This action is used for AJAX requests to filter movies based on the provided criteria.
        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] MovieFilterDTO filter, string? returnUrl = null, string? sectionTitle = "Browse")
        {
            filter.PageSize = 3;

            var result = await _movieService.GetPaginatedMovies(filter);

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