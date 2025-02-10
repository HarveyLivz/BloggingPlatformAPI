using Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.DTOs;

public class RegistrationResponseDto
{
    public string Email { get; set; } = string.Empty;
    public bool IsSuccessful { get; set; }
}
