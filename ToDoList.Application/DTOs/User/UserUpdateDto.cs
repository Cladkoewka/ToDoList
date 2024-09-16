namespace ToDoList.Application.DTOs.User;

public class UserUpdateDto
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string? PasswordHash { get; set; }
}