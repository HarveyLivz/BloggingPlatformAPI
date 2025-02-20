using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePost(CreatePostRequestDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var result = await postService.CreatePost(request, Guid.Parse(userId));
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("delete/{postId}")]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            var authorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authorId))
            {
                return Unauthorized("User not authenticated");
            }

            var result = await postService.DeletePostById(postId, Guid.Parse(authorId));
            return Ok(result);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPostsByUserId()
        {
            var authorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authorIdClaim) || !Guid.TryParse(authorIdClaim, out var authorId))
            {
                return BadRequest(new { message = "Invalid author ID!" });
            }

            var posts = await postService.GetAllPostsByUserId(authorId);

            if (!posts.Any())
            {
                return NotFound(new { message = "No posts found!" });
            }

            return Ok(posts);
        }
    }
}
