using ToDoList.Application.DTOs.Task;

namespace ToDoList.Application.Services.Interfaces;

public interface ITaskService
{
    Task<TaskGetDto?> GetTaskByIdAsync(int id, int userId);
    Task<IEnumerable<TaskGetDto>> GetAllTasksAsync(int userId);
    Task<PaginatedResult<TaskGetDto>> GetPaginatedTasksAsync(int pageNumber, int pageSize, int userId);
    Task<PaginatedResult<TaskGetDto>> GetFilteredTasksAsync(int pageNumber, int pageSize, bool showCompleted,
        int userId);
    Task<IEnumerable<TaskGetDto>> GetTasksByTagsAsync(IEnumerable<int> tagIds);
    Task<TaskGetDto?> CreateTaskAsync(TaskCreateDto taskDto, int userId);
    Task<bool> UpdateTaskAsync(int id, TaskUpdateDto taskDto, int userId);
    Task<bool> DeleteTaskAsync(int id, int userId);
}