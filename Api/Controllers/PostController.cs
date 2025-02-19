using Api.Entities;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController(IPostService postService) : ControllerBase
    {
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


    }
}
