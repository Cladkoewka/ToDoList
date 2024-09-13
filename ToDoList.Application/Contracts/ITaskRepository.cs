using ToDoList.Domain.Entities;
using Task = ToDoList.Domain.Entities.Task;

namespace ToDoList.Application.Contracts;

public interface ITaskRepository
{
    Task<Task?> GetByIdAsync(int id);
    Task<IEnumerable<Task>> GetAllAsync();
    Task<IEnumerable<Task>> GetByTagsAsync(IEnumerable<int> tagIds);
    System.Threading.Tasks.Task AddAsync(Task task);
    System.Threading.Tasks.Task UpdateAsync(Task task);
    System.Threading.Tasks.Task DeleteAsync(Task task);
}