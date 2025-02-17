using Api.Data;
using Api.Entities;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreatePostResponseDto?> CreatePost(CreatePostRequestDto createPostRequestDto, Guid userId)
        {
            // Ensure the logged-in user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                throw new ArgumentException("Invalid user. Please log in again.");
            }

            var post = new Post
            {
                Title = createPostRequestDto.Title,
                Content = createPostRequestDto.Content,
                AuthorId = userId 
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return new CreatePostResponseDto
            {
                Title = post.Title,
                Content = post.Content
            };
        }
    }
}
