namespace ToDoList.Application.DTOs.User;

public class UserUpdateDto
{

    public string Email { get; set; }
    public string Username { get; set; }

    public string? Password { get; set; }
}