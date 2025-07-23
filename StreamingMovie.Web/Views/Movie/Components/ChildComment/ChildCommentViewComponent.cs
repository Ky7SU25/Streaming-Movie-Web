using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.Interfaces;

namespace StreamingMovie.Web.Views.Movie.Components.ChildComment
{
    public class ChildCommentViewComponent : ViewComponent
    {
        private readonly ICommentService _commentService;

        public ChildCommentViewComponent(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int parentId)
        {
            var replies = await _commentService.GetRepliesAsync(parentId);
            return View(replies);
        }
    }
}