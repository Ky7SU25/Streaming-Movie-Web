using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Application.Interfaces;
using System.Security.Claims;

namespace StreamingMovie.Web.Views.Movie.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // POST: /Comment/Create
        [HttpPost]
        public async Task<IActionResult> Create(CommentRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.UserId = int.Parse(userId);

            await _commentService.AddAsync(model);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // POST: /Comment/Update
        [HttpPost]
        public async Task<IActionResult> Update(int commentId, string content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _commentService.UpdateAsync(commentId, content);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // POST: /Comment/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int commentId)
        {
            await _commentService.DeleteAsync(commentId);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
