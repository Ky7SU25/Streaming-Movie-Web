using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;

namespace StreamingMovie.Web.Views.Shared.Components.SidebarTopView
{
    public class SidebarMovie : ViewComponent
    {
        private readonly IMovieService _movieService;
        private readonly IMemoryCache _cache;

        public SidebarMovie(IMovieService movieService, IMemoryCache cache)
        {
            _movieService = movieService;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string period = "day")
        {
            var cacheKey = $"SidebarTopView_{period}";

            if (!_cache.TryGetValue(cacheKey, out List<TopViewMovieDTO> topViewMovies))
            {
                topViewMovies = await _movieService.GetTopViewMoviesAsync(period, 4);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                _cache.Set(cacheKey, topViewMovies, cacheOptions);
            }

            ViewBag.CurrentPeriod = period;
            return View(topViewMovies);
        }
    }
}
