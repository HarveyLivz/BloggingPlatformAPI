using Api.DTOs;
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
        public async Task<ActionResult<string>> Login(LoginRequestDto loginRequestDto)
        {
            var token = await authService.LoginAsync(loginRequestDto);

            if (token is null)
            {
                return BadRequest("Invalid email or password.");
            }

            return Ok(token);
        }

    }
}
