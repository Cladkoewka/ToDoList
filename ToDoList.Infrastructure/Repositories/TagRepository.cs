using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Interfaces;
using ToDoList.Infrastructure.DbContext;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly ApplicationDbContext _context;

    public TagRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Tag?> GetByIdAsync(int id)
    {
        return await _context.Tags
            .Include(tag => tag.TaskTagAssociations)
            .ThenInclude(association => association.Task)
            .FirstOrDefaultAsync(tag => tag.Id == id);
    }
    
    public async Task<Tag?> GetByNameAsync(string name)
    {
        return await _context.Tags
            .Include(tag => tag.TaskTagAssociations)
            .ThenInclude(association => association.Task)
            .FirstOrDefaultAsync(tag => tag.Name == name);
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _context.Tags
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<int> tagIds)
    {
        return await _context.Tags
            .Where(tag => tagIds.Contains(tag.Id))
            .ToListAsync();
    }

    public async Task AddAsync(Tag tag)
    {
        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tag tag)
    {
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Tag tag)
    {
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
    }
}