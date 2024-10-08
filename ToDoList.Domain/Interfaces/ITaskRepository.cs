using Task = ToDoList.Domain.Entities.Task;

namespace ToDoList.Domain.Interfaces;

public interface ITaskRepository
{
    Task<Task?> GetByIdAsync(int id);
    Task<IEnumerable<Task>> GetAllAsync();
    Task<int> GetTaskCount();
    Task<int> GetTaskCount(bool isCompleted);
    Task<IEnumerable<Task>> GetPaginatedTasksAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Task>> GetPaginatedTasksAsync(int pageNumber, int pageSize, bool isCompleted);
    Task<IEnumerable<Task>> GetByTagsAsync(IEnumerable<int> tagIds);
    System.Threading.Tasks.Task AddAsync(Task task);
    System.Threading.Tasks.Task UpdateAsync(Task task);
    System.Threading.Tasks.Task DeleteAsync(Task task);
    
}