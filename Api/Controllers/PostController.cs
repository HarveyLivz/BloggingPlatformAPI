using Api.DTOs;
using Api.Models;
using Api.Services;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController(IPostService postService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<ActionResult<CreatePostResponseDto>> Create([FromBody] CreatePostRequestDto createPostRequsetDto)
        {
            var post = await postService.CreatePost(createPostRequsetDto);

            return post;
        }
    }
}
