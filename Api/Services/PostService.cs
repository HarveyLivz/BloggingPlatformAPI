using Api.Data;
using Api.Entities;
using Api.Models;
using Api.Services.Interfaces;

namespace Api.Services
{
    public class PostService(ApplicationDbContext context) : IPostService
    {
        public async Task<CreatePostResponseDto?> CreatePost(CreatePostRequestDto createPostRequstDto)
        {
            var post = new Post
            {
                Title = createPostRequstDto.Title,
                Content = createPostRequstDto.Content
            };

            return new CreatePostResponseDto
            {
                Title = post.Title,
                Content = post.Content
            };

        }
    }
}
