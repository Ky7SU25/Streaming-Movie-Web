using StreamingMovie.Web.Views.Admin.Controllers;

namespace StreamingMovie.Web.Views.Admin.ViewModels
{
    public class VideoUploadViewModel
    {
        public List<SelectListItemDto> Countries { get; set; } = new();
        public List<SelectListItemDto> Categories { get; set; } = new();
        public List<SelectListItemDto> Directors { get; set; } = new();
        public List<SelectListItemDto> Actors { get; set; } = new();
        public List<SelectListItemDto> VideoServers { get; set; } = new();
        public List<SelectListItemDto> VideoQualities { get; set; } = new();
    }

    public class SelectListItemDto
    {
        public string Value { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
