using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Application.Services;
using StreamingMovie.Domain.Entities;
using System.Security.Claims;

namespace StreamingMovie.Web.Views.Movie.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IUnifiedMovieService _unifiedMovieService;

        public FavoriteController(IFavoriteService favoriteService, IUnifiedMovieService unifiedMovieService)
        {
            _favoriteService = favoriteService;
            _unifiedMovieService = unifiedMovieService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite(string slug)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var unifiedMovie = await _unifiedMovieService.Find(x => x.Slug == slug).FirstOrDefaultAsync();

            if (unifiedMovie == null)
            {
                TempData["error"] = "Movie not found.";
                return RedirectToAction("Details", "Movie", new { slug = slug });
            }

            var favorite = new Favorite
            {
                UserId = userId.Value,
                SeriesId = unifiedMovie.IsSeries ? unifiedMovie.Id : null,
                MovieId = unifiedMovie.IsSeries ? null : unifiedMovie.Id
            };

            var result = await _favoriteService.AddAsync(favorite);
            if (result == null)
            {
                TempData["error"] = "Action failed.";
            }
            else
            {
                TempData["success"] = "Add to favorites.";
            }

            return RedirectToAction("Details", "Movie", new {slug = slug });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFavorite(string slug)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var unifiedMovie = await _unifiedMovieService.Find(x => x.Slug == slug).FirstOrDefaultAsync();

            if (unifiedMovie == null)
            {
                TempData["warning"] = "Movie not found.";
                return RedirectToAction("Details", "Movie", new {slug = slug });
            }

            var favorite = unifiedMovie.IsSeries
                ? await _favoriteService.FindOneAsync(f => f.SeriesId == unifiedMovie.Id && f.UserId == userId)
                : await _favoriteService.FindOneAsync(f => f.MovieId == unifiedMovie.Id && f.UserId == userId);

            if (favorite == null)
            {
                TempData["warning"] = "You can only remove your own favorites.";
            }
            else
            {
                await _favoriteService.DeleteAsync(favorite);
                TempData["success"] = "Removed from favorites.";
            }

            return RedirectToAction("Details", "Movie", new {slug = slug });
        }


        private int? GetUserId()
        {
            var userIdClaims = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaims))
                return null;

            return int.Parse(userIdClaims);
        }
    }
}
