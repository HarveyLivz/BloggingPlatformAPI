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
        Task<CreatePostRequestDto> DeletePostById(Guid postId);
        Task<CreatePostResponseDto?> GetAllPostsByUserId(Guid userId);
        Task<CreatePostRequestDto> GetPostByPostId(Guid postId);


    }
}
