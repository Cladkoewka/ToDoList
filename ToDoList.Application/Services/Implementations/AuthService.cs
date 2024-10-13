using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ToDoList.Application.DTOs.User;
using ToDoList.Application.Services.Interfaces;

namespace ToDoList.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    
    public AuthService(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
        _logger = Log.ForContext<AuthService>();
    }
    
    public async Task<(string? Token, string? Username)> AuthenticateAsync(UserLoginDto loginDto)
    {
        var user = await _userService.GetUserByEmailAsync(loginDto.Email);
    
        if (user == null)
        {
            _logger.Warning("User not found for email {Email}", loginDto.Email);
            return (null, null);
        }

        if (user.PasswordHash != HashPassword(loginDto.Password))
        {
            _logger.Warning("Invalid password for email {Email}", loginDto.Email);
            return (null, null);
        }

        return (GenerateJwtToken(user), user.Username);
    }

    public string GenerateJwtToken(UserGetDto user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")/*"SecretKeySecretKeySecretKeySecretKey"*/;
        
        _logger.Debug($"JWT KEY is {jwtKey}");
        
        var key = Encoding.ASCII.GetBytes(jwtKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    private string HashPassword(string password)
    {
        // use hash algorithm
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(password)); 
    }
}