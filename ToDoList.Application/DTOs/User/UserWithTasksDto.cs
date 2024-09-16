using ToDoList.Application.DTOs.Task;

namespace ToDoList.Application.DTOs.User;

public class UserWithTasksDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public List<TaskGetDto> Tasks { get; set; } = new();
}