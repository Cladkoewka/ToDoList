using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Contracts;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Repositories;

public class ToDoTaskRepository : IToDoTaskRepository
{
    private readonly ApplicationDbContext _context;

    public ToDoTaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ToDoTask?> GetByIdAsync(int id)
    {
        return await _context.ToDoTasks
            .Include(task => task.TaskTagAssociations)
            .ThenInclude(association => association.Tag)
            .FirstOrDefaultAsync(task => task.Id == id);
    }

    public async Task<IEnumerable<ToDoTask>> GetAllAsync()
    {
        return await _context.ToDoTasks
            .ToListAsync();
    }

    public async Task<IEnumerable<ToDoTask>> GetByTagsAsync(IEnumerable<int> tagIds)
    {
        if (!tagIds.Any())
        {
            return Enumerable.Empty<ToDoTask>();
        }

        var taskIds = await _context.TaskTagAssociations
            .Where(association => tagIds.Contains(association.TagId))
            .Select(association => association.ToDoTaskId)
            .Distinct()
            .ToListAsync();

        return await _context.ToDoTasks
            .Where(task => taskIds.Contains(task.Id))
            .ToListAsync();
    }

    public async Task AddAsync(ToDoTask toDoTask)
    {
        await _context.ToDoTasks.AddAsync(toDoTask);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ToDoTask toDoTask)
    {
        _context.ToDoTasks.Update(toDoTask);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ToDoTask toDoTask)
    {
        _context.ToDoTasks.Remove(toDoTask);
        await _context.SaveChangesAsync();
    }
}
