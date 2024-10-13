namespace ToDoList.Application.DTOs.User;

public class UserGetDto
{
    public int Id { get; set; }
    public string Username { get; set; }

    public string Email { get; set; }
    public string PasswordHash { get; set; }
}