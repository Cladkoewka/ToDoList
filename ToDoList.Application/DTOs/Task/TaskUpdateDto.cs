namespace ToDoList.Application.DTOs.Task;

public class TaskUpdateDto
{
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime LastUpdateTime { get; set; }
    public bool IsCompleted { get; set; }

    public List<int> TagIds { get; set; } = new();
}