using StreamingMovie.Application.DTOs;
using StreamingMovie.Domain.Entities;
using EntityCategory = StreamingMovie.Domain.Entities.Category;

namespace StreamingMovie.Web.Views.Movie.ViewModels
{
    public class FilterPanelViewModel
    {
        public List<EntityCategory> Categories { get; set; } = new();
        public List<Country> Countries { get; set; } = new();
        public List<int> Years { get; set; } = new();
        public MovieFilterDTO Filter { get; set; } = new(); 
    }
}
