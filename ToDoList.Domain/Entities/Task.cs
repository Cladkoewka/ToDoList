
namespace ToDoList.Domain.Entities;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public bool IsCompleted { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<TaskTagAssociation> TaskTagAssociations { get; set; } = new List<TaskTagAssociation>();
}