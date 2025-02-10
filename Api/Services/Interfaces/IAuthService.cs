using Api.DTOs;
using Api.Entities;

namespace Api.Services
{
    public interface IAuthService
    {
        public bool IsUnique(string email);
        public Task<User?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        public Task<string?> LoginAsync(LoginRequestDto loginRequestDto);
    }
}
