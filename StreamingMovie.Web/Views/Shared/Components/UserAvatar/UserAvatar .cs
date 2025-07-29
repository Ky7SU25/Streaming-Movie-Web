using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Web.Views.Shared.Components.UserAvatar;

public class UserAvatar : ViewComponent
{
    private readonly UserManager<User> _userManager;

    public UserAvatar(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        string avatarUrl = "/uploads/avatars/default.jpg";

        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (!string.IsNullOrEmpty(user?.AvatarUrl))
            {
                avatarUrl = user.AvatarUrl;
            }
        }

        return View("Default", avatarUrl);
    }
}
