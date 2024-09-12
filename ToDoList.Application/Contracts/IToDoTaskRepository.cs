using ToDoList.Domain.Entities;

namespace ToDoList.Application.Contracts;

public interface IToDoTaskRepository
{
    Task<ToDoTask?> GetByIdAsync(int id);
    Task<IEnumerable<ToDoTask>> GetAllAsync();
    Task<IEnumerable<ToDoTask>> GetByTagsAsync(IEnumerable<int> tagIds);
    Task AddAsync(ToDoTask toDoTask);
    Task UpdateAsync(ToDoTask toDoTask);
    Task DeleteAsync(ToDoTask toDoTask);
}