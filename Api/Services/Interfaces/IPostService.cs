using Api.Models;

namespace Api.Services.Interfaces
{
    public interface IPostService
    {
        Task<CreatePostResponseDto?> CreatePost(CreatePostRequestDto createPostRequstDto);
    }
}
