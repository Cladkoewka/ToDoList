using ToDoList.Application.DTOs.Task;
using ToDoList.Domain.Entities;
using Task = ToDoList.Domain.Entities.Task;

namespace ToDoList.Application.Services.Mapping;

public interface ITaskMapper
{
    TaskGetDto MapToGetDto(Task task);
    Task MapToEntity(TaskCreateDto taskDto, IEnumerable<Tag> tags);
    Task MapToEntity(TaskUpdateDto taskDto, Task task,  IEnumerable<Tag> tags);
}