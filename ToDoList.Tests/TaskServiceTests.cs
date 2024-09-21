using ToDoList.Application.DTOs.Task;
using ToDoList.Application.Services.Implementations;
using ToDoList.Application.Services.Mapping;
using ToDoList.Domain.Interfaces;
using Xunit;

namespace ToDoList.Tests;

public class TaskServiceTests
{
    private readonly ITaskRepository _taskRepositoryMock;
    private readonly ITagRepository _tagRepositoryMock;
    private readonly ITaskMapper _taskMapperMock;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _taskService = new TaskService(null,null, null);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskNotFound()
    {
        int taskId = 1;

        var result = _taskService.GetTaskByIdAsync(taskId);
        
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
    
    /*
    [Fact]
    public async void GetTaskByIdAsync_TaskExists_ReturnsTask()
    {
        var task = new Domain.Entities.Task { Id = 1 };
        _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(task);

        var result = await _taskService.GetTaskByIdAsync(1);

        Assert.NotNull(result);
        _taskRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }
    */
}