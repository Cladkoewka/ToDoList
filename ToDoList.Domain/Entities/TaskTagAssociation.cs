namespace ToDoList.Domain.Entities;

public class TaskTagAssociation
{
    public int ToDoTaskId { get; set; }
    public ToDoTask ToDoTask { get; set; }
    
    public int TagId { get; set; }
    public Tag Tag { get; set; }
}