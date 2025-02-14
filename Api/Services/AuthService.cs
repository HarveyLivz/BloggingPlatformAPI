using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Models;
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

        return await CreateTokenResponse(user);

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

    private async Task<LoginResponseDto> CreateTokenResponse(User? user)
    {
        return new LoginResponseDto
        {
            AccessToken = CreateToken(user),
            RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
        };
    }

    public async Task<LoginResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var user = await ValidateRefreshTokenAsync(refreshTokenRequestDto.UserId, refreshTokenRequestDto.RefreshToken);

        if (user is null)
        {
            return null;
        }

        return await CreateTokenResponse(user);
    }


    private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var user = await context.Users.FindAsync(userId);

        if (user is null || user.RefreshToken != refreshToken 
            || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        return user;

    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(10);
        await context.SaveChangesAsync();
        return refreshToken;
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
