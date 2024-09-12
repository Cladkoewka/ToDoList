using ToDoList.Domain.Entities;

namespace ToDoList.Application.Contracts;

public interface ITaskTagAssociationRepository
{
    Task<TaskTagAssociation?> GetByTaskIdAndTagIdAsync(int taskId, int tagId);
    Task<IEnumerable<TaskTagAssociation>> GetByTaskIdAsync(int taskId);
    Task<IEnumerable<TaskTagAssociation>> GetByTagIdAsync(int tagId);
    Task AddAsync(TaskTagAssociation taskTagAssociation);
    Task DeleteAsync(TaskTagAssociation taskTagAssociation);
}