using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using System.Security.Claims;

namespace StreamingMovie.Web.Views.Movie.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        // POST: /Rating/Create
        [HttpPost]
        public async Task<IActionResult> Create(RatingRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.UserId = int.Parse(userId);

            await _ratingService.AddAsync(model);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // POST: /Rating/Update
        [HttpPost]
        public async Task<IActionResult> Update(RatingRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _ratingService.UpdateAsync(model);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // POST: /Rating/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int ratingId)
        {
            await _ratingService.DeleteAsync(ratingId);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}

