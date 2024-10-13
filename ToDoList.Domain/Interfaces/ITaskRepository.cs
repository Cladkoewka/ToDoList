using Task = ToDoList.Domain.Entities.Task;

namespace ToDoList.Domain.Interfaces;

public interface ITaskRepository
{
    Task<Task?> GetByIdAsync(int id);
    Task<IEnumerable<Task>> GetAllAsync();
    Task<IEnumerable<Task>> GetAllByUserIdAsync(int userId);
    Task<int> GetTaskCount(int userId);
    Task<int> GetTaskCount(bool isCompleted, int userId);
    Task<IEnumerable<Task>> GetPaginatedTasksAsync(int pageNumber, int pageSize, int userId);
    Task<IEnumerable<Task>> GetPaginatedTasksAsync(int pageNumber, int pageSize, bool isCompleted, int userId);
    Task<IEnumerable<Task>> GetByTagsAsync(IEnumerable<int> tagIds);
    System.Threading.Tasks.Task AddAsync(Task task);
    System.Threading.Tasks.Task UpdateAsync(Task task);
    System.Threading.Tasks.Task DeleteAsync(Task task);
}