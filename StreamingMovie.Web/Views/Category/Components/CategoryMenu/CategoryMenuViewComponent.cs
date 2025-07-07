using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Domain.Interfaces;
using EntityCategory = StreamingMovie.Domain.Entities.Category;

namespace StreamingMovie.Web.Views.Category.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly IGenericService<EntityCategory> _categoryService; // Fully qualify 'Category' to avoid ambiguity

        public CategoryMenuViewComponent(IGenericService<EntityCategory> categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            return View("~/Views/Category/Components/CategoryMenu/Default.cshtml", categories);
        }
    }
}
