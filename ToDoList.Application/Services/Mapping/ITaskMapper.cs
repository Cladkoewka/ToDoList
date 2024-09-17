using ToDoList.Application.DTOs.Task;
using Task = ToDoList.Domain.Entities.Task;

namespace ToDoList.Application.Services.Mapping;

public interface ITaskMapper
{
    TaskGetDto MapToGetDto(Task task);
    Task MapToEntity(TaskCreateDto taskDto);
    Task MapToEntity(TaskUpdateDto taskDto, Task task);
}