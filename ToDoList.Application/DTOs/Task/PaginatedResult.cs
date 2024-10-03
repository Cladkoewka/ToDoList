namespace ToDoList.Application.DTOs.Task;

public class PaginatedResult<T>
{
    public int TotalCount { get; set; }
    public List<T> Tasks { get; set; }
}