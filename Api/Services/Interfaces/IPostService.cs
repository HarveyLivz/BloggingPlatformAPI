using Api.Models;

namespace Api.Services.Interfaces
{
    public interface IPostService
    {
        Task<CreatePostResponseDto?> CreatePost(CreatePostRequestDto createPostRequestDto, Guid userId);
        Task<CreatePostResponseDto?> GetPostById(Guid userId);
    }
}
