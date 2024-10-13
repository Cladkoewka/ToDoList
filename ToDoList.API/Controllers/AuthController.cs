using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.DTOs.User;
using ToDoList.Application.Services.Interfaces;

namespace ToDoList.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateDto userDto)
    {
        var user = await _userService.CreateUserAsync(userDto);
        if (user == null)
        {
            return BadRequest("User already exists");
        }

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
        var (token, username) = await _authService.AuthenticateAsync(loginDto);
        if (token == null)
        {
            return Unauthorized("Invalid credentials");
        }
        
        Response.Cookies.Append("authToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(1)
        });
        
        return Ok(new { Token = token, Username = username });
    }
}