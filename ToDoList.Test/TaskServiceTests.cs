using ToDoList.Application.DTOs.Task;
using ToDoList.Application.Services.Implementations;
using Xunit;

namespace ToDoList.Test;

public class TaskServiceTests
{
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _taskService = new TaskService(null,null, null);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskNotFound()
    {
        int taskId = 1;

        var result = await _taskService.GetTaskByIdAsync(taskId);
        
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateTaskAcync_ShouldReturnTask_WhenTaskCreated()
    {
        var taskCreateDto = new TaskCreateDto
        {
            Title = "New Task",
            CreatedTime = DateTime.Now,
            Description = "Description",
            TagIds = new List<int>() { 1, 2 },
            UserId = 1

        };

        var result = await _taskService.CreateTaskAsync(taskCreateDto);

        Assert.NotNull(result);
        Assert.Equal("New Task", result.Title);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldReturnFalse_WhenTaskNotFound()
    {
        int taskId = 1;

        var result = await _taskService.DeleteTaskAsync(taskId);
        
        Assert.False(result);
    }
}