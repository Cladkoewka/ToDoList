using ToDoList.Application.DTOs.Tag;
using ToDoList.Application.DTOs.Task;
using ToDoList.Domain.Entities;
using Task = ToDoList.Domain.Entities.Task;

namespace ToDoList.Application.Services.Mapping;

public class TaskMapper : ITaskMapper
{
    public TaskGetDto MapToGetDto(Task task)
    {
        return new TaskGetDto
        {
            Description = task.Description,
            CreatedTime = task.CreatedTime,
            Id = task.Id,
            IsCompleted = task.IsCompleted,
            LastUpdateTime = task.LastUpdateTime,
            Title = task.Title,
            UserId = task.UserId,
            Tags = task.TaskTagAssociations
                .Select(association => new TagGetDto
                {
                    Id = association.Tag.Id,
                    Name = association.Tag.Name
                })
                .ToList()
        };
    }

    public Task MapToEntity(TaskCreateDto taskDto, IEnumerable<Tag> tags)
    {
        return new Task
        {
            Description = taskDto.Description,
            CreatedTime = taskDto.CreatedTime,
            LastUpdateTime = taskDto.CreatedTime,
            Title = taskDto.Title,
            UserId = taskDto.UserId,
            TaskTagAssociations = tags.Select(tag => new TaskTagAssociation
            {
                TagId = tag.Id,
            }).ToList()
        };
    }

    public Task MapToEntity(TaskUpdateDto taskDto, Task task, IEnumerable<Tag> tags)
    {
        task.Description = taskDto.Description;
        task.Title = taskDto.Title;
        task.LastUpdateTime = taskDto.LastUpdateTime;
        task.IsCompleted = taskDto.IsCompleted;
        
        task.TaskTagAssociations.Clear();

        foreach (var tag in tags)
        {
            task.TaskTagAssociations.Add(new TaskTagAssociation
            {
                TaskId = task.Id,
                TagId = tag.Id
            });
        }

        return task;
    }
}