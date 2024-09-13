using ToDoList.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Application.Contracts;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(int id);
    Task<IEnumerable<Tag>> GetAllAsync();
    Task AddAsync(Tag tag);
    Task UpdateAsync(Tag tag);
    Task DeleteAsync(Tag tag);
}