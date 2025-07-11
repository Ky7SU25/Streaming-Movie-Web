using Microsoft.AspNetCore.Mvc;

namespace StreamingMovie.Web.Views.Shared.Components.SidebarTopView
{
    public class SidebarMovie : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
