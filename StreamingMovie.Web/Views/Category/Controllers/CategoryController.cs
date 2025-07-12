using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.Interfaces;

namespace StreamingMovie.Web.Views.Category.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet]
        [Route("categories")]
        public async Task<IActionResult> Categories(string returnUrl = null)
        {
            var categories = await _categoryService.GetAllAsync();

            ViewData["ReturnUrl"] = returnUrl;
            return View(categories);
        }
    }
}
