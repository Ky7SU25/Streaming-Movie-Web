using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StreamingMovie.Application.Interfaces;
using EntityCategory = StreamingMovie.Domain.Entities.Category;

namespace StreamingMovie.Web.Views.Category.Components.CategoryMenu
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService; // Fully qualify 'Category' to avoid ambiguity
        private readonly IMemoryCache _cache;

        public CategoryMenuViewComponent(ICategoryService categoryService, IMemoryCache cache)
        {
            _categoryService = categoryService;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            const string cacheKey = "CategoryMenu_Cached";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<EntityCategory> categories))
            {
                categories = await _categoryService.GetAllAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)); 

                _cache.Set(cacheKey, categories, cacheOptions);
            }

            return View("~/Views/Category/Components/CategoryMenu/Default.cshtml", categories);
        }
    }
}
