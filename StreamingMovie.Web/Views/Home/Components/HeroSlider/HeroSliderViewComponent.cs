using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;

public class HeroSliderViewComponent : ViewComponent
{
    private readonly IUnifiedMovieService _unifiedMovieService;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;

    public HeroSliderViewComponent(
        IUnifiedMovieService unifiedMovieService,
        IMemoryCache cache,
        IMapper mapper)
    {
        _unifiedMovieService = unifiedMovieService;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        const string cacheKey = "PopularMovie_Cached";

        if (!_cache.TryGetValue(cacheKey, out IEnumerable<UnifiedMovieDTO> movieDTOs))
        {
            var movies = await _unifiedMovieService.Query()
                .OrderByDescending(m => m.ImdbRating)
                .Take(3)
                .ToListAsync();

            movieDTOs = _mapper.Map<IEnumerable<UnifiedMovieDTO>>(movies);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            _cache.Set(cacheKey, movieDTOs, cacheOptions);
        }

        return View(movieDTOs);
    }
}
