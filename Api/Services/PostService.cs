using Api.Data;
using Api.Entities;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<object> DeletePostById(Guid postId, Guid authorId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                return new { message = "Post not found!", StatusCode = 404 };
            }

            if (post.AuthorId != authorId)  // Ensure the Post entity has authorId
            {
                return new { message = "You can only delete your own posts!", StatusCode = 403 };
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return new { message = "Post deleted successfully", StatusCode = 200 };
        }

        public Task<CreatePostResponseDto?> GetAllPostsByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<CreatePostRequestDto> GetPostByPostId(Guid postId)
        {
            throw new NotImplementedException();
        }
    }
}
