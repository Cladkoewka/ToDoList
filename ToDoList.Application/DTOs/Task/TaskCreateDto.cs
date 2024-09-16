namespace ToDoList.Application.DTOs.Task;

public class TaskCreateDto
{
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DueTime { get; set; }

    public int UserId { get; set; }

    public List<int> TagIds { get; set; } = new();
}