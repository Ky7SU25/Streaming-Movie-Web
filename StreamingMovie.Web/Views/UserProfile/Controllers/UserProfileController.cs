using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Web.Views.UserProfile.ViewModels;

namespace StreamingMovie.Web.Views.UserProfile.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserService _userService;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserProfileController(IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString);

            var user = await _userManager.FindByIdAsync(userIdString);
            if (user == null)
            {
                return NotFound();
            }

            var userProfileViewModel = new UserProfileViewModel
            {
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl,
                SubscriptionType = user.SubscriptionType,
                SubscriptionStartDate = user.SubscriptionStartDate,
                SubscriptionEndDate = user.SubscriptionEndDate,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return View(userProfileViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> EditUserProfile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString);
            var user = await _userManager.FindByIdAsync(userIdString);
            if (user == null)
            {
                return NotFound();
            }
            var userProfileViewModel = new EditUserViewModel
            {
                UserId = user.Id,
                FullName = user.FullName,
                CurrentAvatar = user.AvatarUrl,

            };
            return View(userProfileViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserViewModel model)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString);
            var user = await _userManager.FindByIdAsync(userIdString);
            if (user == null)
            {
                return NotFound();
            }

            if (model.AvatarUrl != null && model.AvatarUrl.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.AvatarUrl.FileName);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.AvatarUrl.CopyToAsync(stream);
                }

                user.AvatarUrl = fileName;
            }
            else
            {
                // Giữ lại ảnh cũ
                user.AvatarUrl = model.CurrentAvatar;
            }

            user.FullName = model.FullName;
            await _userService.UpdateAsync(user);

            return RedirectToAction("UserProfile", "UserProfile");
        }


    }
}
