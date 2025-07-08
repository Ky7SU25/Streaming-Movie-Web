using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.Services;

namespace StreamingMovie.Web.Views.Movie.Controllers
{
    public class DetailMovieController : Controller
    {
        //private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly UserManager<IdentityUser> _userManager;
        private DetailMovieService _detailMovieService;
        public DetailMovieController(DetailMovieService detailMovieService)
        {
            _detailMovieService = detailMovieService;
        }
        //public async Task<IActionResult> Details(int id)
        //{
        //    if (id <= 0)
        //    {
        //        return NotFound();
        //    }
        //    var movie = await _detailMovieService.GetMovieByIdAsync(id);
        //    return View(movie);
        //}
    }
}
