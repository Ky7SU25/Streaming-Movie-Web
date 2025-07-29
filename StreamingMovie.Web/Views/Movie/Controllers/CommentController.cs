using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using StreamingRating.Application.Services;
using System.Security.Claims;

namespace StreamingMovie.Web.Views.Movie.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> LoadComments(string slug, int page = 1)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? userId = null;

            if (int.TryParse(userIdClaim, out int parsedUserId))
            {
                userId = parsedUserId;
            }

            var pagedRating = await _commentService.PaginateBySlugAsync(slug, parsedUserId, page);
            var model = Tuple.Create(pagedRating, slug);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Movie/_CommentSection.cshtml", model);
            }
            return RedirectToAction("Watching", "Movie", new { slug = slug, page = page });
        }

        // POST: /Comment/Create
        [HttpPost]
        public async Task<IActionResult> Create(CommentRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            model.UserId = userId.Value;

            await _commentService.AddAsync(model);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // POST: /Comment/Update
        [HttpPost]
        public async Task<IActionResult> Update(int commentId, string content)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (content == null)
            {
                return BadRequest("Invalid rating data.");
            }

            var result = await _commentService.UpdateAsync(commentId, content);
            if (result == null)
            {
                TempData["error"] = "Failed to edit comment.";
            }
            else
            {
                TempData["success"] = "Comment edited.";
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // POST: /Comment/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int commentId)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var isDeleted = await _commentService.DeleteCommentWithChildrenAsync(commentId);
            if (!isDeleted)
            {
                TempData["error"] = "Failed to delete comment.";
            }
            else
            {
                TempData["success"] = "Comment deleted.";
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }


        private int? GetUserId()
        {
            var userIdClaims = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaims))
                return null;

            return int.Parse(userIdClaims);
        }
    }
}
