using Api.DTOs;
using Api.Models;

namespace Api.Services
{
    public interface IAuthService
    {
        bool IsUnique(string email);
        Task<RegistrationResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<LoginResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequestDto);
    }
}
