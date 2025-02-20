using Api.Entities;
using Api.Models;

namespace Api.Services.Interfaces
{
    public interface IPostService
    {
        // TODO: Create, Update, Delete Post
        // TODO: Get All Posts by User ID
        // TODO: Get Post by Post ID
        // TODO: Get All Posts
        Task<CreatePostResponseDto?> CreatePost(CreatePostRequestDto createPostRequestDto, Guid userId);
        Task<object> DeletePostById(Guid postId, Guid authorId);
        Task<List<Post>> GetAllPostsByUserId(Guid authorId);
        Task<CreatePostRequestDto> GetPostByPostId(Guid postId);


    }
}
