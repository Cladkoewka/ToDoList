using ToDoList.Application.DTOs.Task;
using Task = ToDoList.Domain.Entities.Task;

namespace ToDoList.Application.Services.Mapping;

public class TaskMapper : ITaskMapper
{
    public TaskGetDto MapToGetDto(Task task)
    {
        return new TaskGetDto
        {
            Description = task.Description,
            DueTime = task.DueTime,
            Id = task.Id,
            IsCompleted = task.IsCompleted,
            LastUpdateTime = task.LastUpdateTime,
            Title = task.Title,
        };
    }

    public Task MapToEntity(TaskCreateDto taskDto)
    {
        return new Task
        {
            Description = taskDto.Description,
            DueTime = taskDto.DueTime,
            LastUpdateTime = taskDto.DueTime,
            Title = taskDto.Title,
            UserId = taskDto.UserId
        };
    }

    public Task MapToEntity(TaskUpdateDto taskDto, Task task)
    {
        task.Description = taskDto.Description;
        task.Title = taskDto.Title;
        task.LastUpdateTime = taskDto.LastUpdateTime;

        return task;
    }
}