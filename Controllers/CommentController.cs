using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Service.CommentService;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    [Route("api/comment")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [Route("{issueId}/create")]
        public async Task<IActionResult> CreateComment(string description, Guid issueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await _commentService.CreateComment(description, issueId, user.Value));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("issue={issueId}/update/comment={commentId}")]
        public async Task<IActionResult> UpdateComment(string description, Guid commentId, Guid issueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await _commentService.UpdateComment(description, commentId, issueId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("issue={issueId}/comment={commentId}")]
        public async Task<IActionResult> RetrieveCommentById(Guid commentId, Guid issueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await _commentService.GetComment(commentId, issueId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("issue={issueId}/comments")]
        public async Task<IActionResult> RetrieveComments( Guid issueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await _commentService.GetAllComments( issueId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("issue={issueId}/delete/comment={commentId}")]
        public async Task<IActionResult> DeleteComments(Guid commentId, Guid issueId)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(await _commentService.DeleteComment(commentId, issueId, user.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
