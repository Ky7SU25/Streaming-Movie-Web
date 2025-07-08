using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.Services;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Web.Views.Movie.Controllers
{
    public class MovieController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private DetailMovieService _detailMovieService;
        public MovieController(SignInManager<User> signInManager, UserManager<User> userManager, DetailMovieService detailMovieService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _detailMovieService = detailMovieService;
        }

        //public IActionResult Details(string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    return View();
        //}
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var movie = await _detailMovieService.GetMovieByIdAsync(id);
            return View(movie);
        }
        public IActionResult Matching(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
    }
}
