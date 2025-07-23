using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.Interfaces;

namespace StreamingMovie.Web.Views.Movie.Components.Rating
{
    public class RatingViewComponent : ViewComponent 
    {
        private readonly IRatingService _ratingService;

        public RatingViewComponent(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string slug, int page = 1, int pageSize = 5)
        {
            var pagedRating = await _ratingService.PaginateBySlugAsync(slug, page, pageSize);
            return View(pagedRating); 
        }
    }
}
