using Microsoft.AspNetCore.Mvc;

namespace StreamingMovie.Web.Views.Shared.Components.SidebarTopView
{
    public class SidebarComment : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
