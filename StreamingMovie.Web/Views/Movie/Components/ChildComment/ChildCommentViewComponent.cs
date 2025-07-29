using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Web.Views.Movie.Components.ChildComment
{
    public class ChildCommentViewComponent : ViewComponent
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public ChildCommentViewComponent(ICommentService commentService, UserManager<User> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int parentId, string targetType, int targetId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userId = user?.Id;

            var replies = await _commentService.GetRepliesAsync(parentId, userId);
            ViewBag.TargetType = targetType;
            ViewBag.TargetId = targetId;
            ViewBag.ParentId = parentId;

            return View(replies);
        }
    }
}