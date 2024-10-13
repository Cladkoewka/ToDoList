using ToDoList.Application.DTOs.User;

namespace ToDoList.Application.Services.Interfaces;

public interface IAuthService
{
    Task<(string? Token, string? Username)> AuthenticateAsync(UserLoginDto loginDto);
    string GenerateJwtToken(UserGetDto user);
}