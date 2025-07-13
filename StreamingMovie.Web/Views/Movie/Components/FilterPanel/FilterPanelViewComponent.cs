using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Web.Views.Movie.ViewModels;

namespace StreamingMovie.Web.Views.Movie.Components.FilterPanel
{
    public class FilterPanelViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly ICountryService _countryService;

        public FilterPanelViewComponent(ICategoryService categoryService, ICountryService countryService)
        {
            _categoryService = categoryService;
            _countryService = countryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(MovieFilterDTO filter = null)
        {
            var categories = await _categoryService.GetAllAsync();
            var countries = await _countryService.GetAllAsync();
            var years = Enumerable.Range(DateTime.Now.Year - 9, 10).Reverse().ToList();
            var filterPanelViewModel = new FilterPanelViewModel
            {
                Categories = categories.ToList(),
                Countries = countries.ToList(),
                Years = years,
                Filter = filter
            };
            return View("~/Views/Movie/Components/FilterPanel/Default.cshtml", filterPanelViewModel);
        }
    }
}
