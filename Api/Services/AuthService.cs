using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Data;
using Api.DTOs;
using Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class AuthService(ApplicationDbContext context, IConfiguration configuration) : IAuthService
{
    public bool IsUnique(string email)
    {
        if (context.Users.Any(u => u.Email == email))
        {
            return false;
        }

        return true;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == loginRequestDto.Email);

        if (user is null)
        {
            return null;
        }

        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginRequestDto.Password)
            == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return new LoginResponseDto
        {
            Email = user.Email,
            Token = CreateToken(user)
        };
    }

    public async Task<RegistrationResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto)
    {
        if (await context.Users.AnyAsync(u => u.Email == registrationRequestDto.Email))
        {
            return new RegistrationResponseDto
            {
                Email = registrationRequestDto.Email,
                IsSuccessful = false
            };
        }

        var user = new User();

        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(user, registrationRequestDto.Password);

        user.Email = registrationRequestDto.Email;
        user.PasswordHash = hashedPassword;


        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new RegistrationResponseDto
        {
            Email = user.Email,
            IsSuccessful = true
        };
    }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

        var keyString = configuration.GetValue<string>("JwtSettings:Key");
        if (string.IsNullOrEmpty(keyString))
        {
            throw new InvalidOperationException("JWT Key is missing from configuration.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("JwtSettings:Issuer"),
            audience: configuration.GetValue<string>("JwtSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
}
