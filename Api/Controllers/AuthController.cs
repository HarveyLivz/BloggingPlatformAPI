using Api.DTOs;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponseDto>> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var user = await authService.RegisterAsync(registrationRequestDto);

            if (user is null)
            {
                return BadRequest("Email already exist.");
            }

            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
        {
            var result = await authService.LoginAsync(loginRequestDto);

            if (result is null)
            {
                return BadRequest("Invalid email or password.");
            }

            return Ok(result);
        }

        [HttpPost("refreshtoken")]
        public async Task<ActionResult<LoginResponseDto>> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var result = await authService.RefreshTokenAsync(refreshTokenRequestDto);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
            {
                return Unauthorized("Invalid refresh token.");
            }

            return Ok(result);
        }

    }
}
