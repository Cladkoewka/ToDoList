using ToDoList.Application.DTOs.Tag;

namespace ToDoList.Application.DTOs.Task;

public class TaskGetDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public bool IsCompleted { get; set; }
    public int UserId { get; set; }
    public List<TagGetDto> Tags { get; set; }
}