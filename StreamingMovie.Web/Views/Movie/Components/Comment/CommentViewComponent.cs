using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.Interfaces;

namespace StreamingMovie.Web.Views.Movie.Components.Comment
{
    public class CommentViewComponent : ViewComponent
    {
        private readonly ICommentService _commentService;

        public CommentViewComponent(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string slug, int? episodeId, int page = 1, int pageSize = 20)
        {
            var pagedComment = await _commentService.PaginateBySlugAsync(slug, episodeId, page, pageSize);
            return View(pagedComment);
        }
    }
}