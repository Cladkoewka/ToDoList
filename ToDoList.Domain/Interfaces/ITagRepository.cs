using ToDoList.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Domain.Interfaces;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(int id);
    Task<Tag?> GetByNameAsync(string name);
    Task<IEnumerable<Tag>> GetAllAsync();
    Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<int> tagIds);
    Task AddAsync(Tag tag);
    Task UpdateAsync(Tag tag);
    Task DeleteAsync(Tag tag);
}