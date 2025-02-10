using Api.DTOs;
using Api.Entities;

namespace Api.Services
{
    public interface IAuthService
    {
        public bool IsUnique(string email);
        public Task<RegistrationResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        public Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
    }
}
