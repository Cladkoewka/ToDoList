using System.ComponentModel.DataAnnotations;

namespace ToDoList.Domain.Entities;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    
    public ICollection<ToDoTask> Tasks { get; set; } = new List<ToDoTask>();
}