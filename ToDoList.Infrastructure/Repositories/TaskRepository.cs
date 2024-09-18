using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Interfaces;
using ToDoList.Infrastructure.DbContext;
using Task = ToDoList.Domain.Entities.Task;

namespace ToDoList.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Task?> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .Include(task => task.TaskTagAssociations)
            .ThenInclude(association => association.Tag)
            .FirstOrDefaultAsync(task => task.Id == id);
    }

    public async Task<IEnumerable<Task>> GetAllAsync()
    {
        return await _context.Tasks
            .Include(task => task.TaskTagAssociations)
            .ThenInclude(association => association.Tag)
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetByTagsAsync(IEnumerable<int> tagIds)
    {
        if (!tagIds.Any())
        {
            return Enumerable.Empty<Task>();
        }

        var taskIds = await _context.TaskTagAssociations
            .Where(association => tagIds.Contains(association.TagId))
            .Select(association => association.TaskId)
            .Distinct()
            .ToListAsync();

        return await _context.Tasks
            .Where(task => taskIds.Contains(task.Id))
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task AddAsync(Task task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task UpdateAsync(Task task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task DeleteAsync(Task task)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
}
