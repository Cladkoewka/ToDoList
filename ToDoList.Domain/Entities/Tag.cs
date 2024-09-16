
namespace ToDoList.Domain.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<TaskTagAssociation> TaskTagAssociations { get; set; } = new List<TaskTagAssociation>();
}