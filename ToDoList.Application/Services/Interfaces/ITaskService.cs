using ToDoList.Application.DTOs.Task;

namespace ToDoList.Application.Services.Interfaces;

public interface ITaskService
{
    Task<TaskGetDto?> GetTaskByIdAsync(int id);
    Task<IEnumerable<TaskGetDto>> GetAllTasksAsync();
    Task<PaginatedResult<TaskGetDto>> GetPaginatedTasksAsync(int pageNumber, int pageSize);
    Task<PaginatedResult<TaskGetDto>> GetFilteredTasksAsync(int pageNumber, int pageSize, bool showCompleted);
    Task<IEnumerable<TaskGetDto>> GetTasksByTagsAsync(IEnumerable<int> tagIds);
    Task<TaskGetDto?> CreateTaskAsync(TaskCreateDto taskDto);
    Task<bool> UpdateTaskAsync(int id, TaskUpdateDto taskDto);
    Task<bool> DeleteTaskAsync(int id);
}