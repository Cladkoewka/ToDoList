using System.ComponentModel.DataAnnotations;

namespace ToDoList.Domain.Entities;

public class Tag
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }

    public ICollection<TaskTagAssociation> TaskTagAssociations { get; set; } = new List<TaskTagAssociation>();
}