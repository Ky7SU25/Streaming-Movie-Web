using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Web.Views.Movie.Components.Comment
{
    public class CommentViewComponent : ViewComponent
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public CommentViewComponent(ICommentService commentService, UserManager<User> userManager)
        {
            _commentService = commentService;
             _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string slug, int? episodeId, string targetType, int targetId, int page = 1, int pageSize = 10)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userId = user?.Id;

            var pagedComment = await _commentService.PaginateBySlugAsync(slug, episodeId, userId, page, pageSize);
            ViewBag.TargetType = targetType;
            ViewBag.TargetId = targetId;
            return View(pagedComment);
        }
    }
}