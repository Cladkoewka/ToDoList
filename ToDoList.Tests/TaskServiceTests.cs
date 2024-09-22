using Moq;
using ToDoList.Application.DTOs.Task;
using ToDoList.Application.Services.Implementations;
using ToDoList.Application.Services.Mapping;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Interfaces;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<ITaskMapper> _taskMapperMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _tagRepositoryMock = new Mock<ITagRepository>();
            _taskMapperMock = new Mock<ITaskMapper>();

            _taskService = new TaskService(
                _taskRepositoryMock.Object,
                _tagRepositoryMock.Object,
                _taskMapperMock.Object
            );
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskNotFound()
        {
            // Arrange
            int taskId = 1;
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId))
                               .ReturnsAsync((Domain.Entities.Task?)null);

            // Act
            var result = await _taskService.GetTaskByIdAsync(taskId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldReturnFalse_WhenTaskNotFound()
        {
            // Arrange
            int taskId = 1;
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId))
                               .ReturnsAsync((Domain.Entities.Task?)null);

            // Act
            var result = await _taskService.DeleteTaskAsync(taskId);

            // Assert
            Assert.False(result);
        }
        
        [Fact]
        public async Task CreateTaskAsync_ShouldReturnTask_WhenTaskCreated()
        {
            // Arrange
            var taskCreateDto = new TaskCreateDto
            {
                Title = "New Task",
                Description = "Test Description",
                TagIds = new List<int> { 1, 2 }
            };

            var tags = new List<Tag> { new Tag { Id = 1 }, new Tag { Id = 2 } };
            var taskEntity = new Domain.Entities.Task { Id = 1, Title = "New Task", Description = "Test Description" };
            var taskGetDto = new TaskGetDto { Id = 1, Title = "New Task", Description = "Test Description" };

            _tagRepositoryMock.Setup(repo => repo.GetTagsByIdsAsync(taskCreateDto.TagIds))
                              .ReturnsAsync(tags);
            _taskMapperMock.Setup(mapper => mapper.MapToEntity(taskCreateDto, tags))
                           .Returns(taskEntity);
            _taskRepositoryMock.Setup(repo => repo.AddAsync(taskEntity));
            _taskMapperMock.Setup(mapper => mapper.MapToGetDto(taskEntity))
                           .Returns(taskGetDto);

            // Act
            var result = await _taskService.CreateTaskAsync(taskCreateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Task", result.Title);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldReturnTrue_WhenTaskUpdated()
        {
            // Arrange
            int taskId = 1;
            var taskUpdateDto = new TaskUpdateDto
            {
                Title = "Updated Task",
                Description = "Updated Description",
                TagIds = new List<int> { 1, 2 }
            };

            var existingTask = new Domain.Entities.Task { Id = taskId, Title = "Old Task", Description = "Old Description" };
            var tags = new List<Tag> { new Tag { Id = 1 }, new Tag { Id = 2 } };
            var updatedTaskEntity = new Domain.Entities.Task { Id = taskId, Title = "Updated Task", Description = "Updated Description" };

            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId))
                               .ReturnsAsync(existingTask);
            _tagRepositoryMock.Setup(repo => repo.GetTagsByIdsAsync(taskUpdateDto.TagIds))
                              .ReturnsAsync(tags);
            _taskMapperMock.Setup(mapper => mapper.MapToEntity(taskUpdateDto, existingTask, tags))
                           .Returns(updatedTaskEntity);
            _taskRepositoryMock.Setup(repo => repo.UpdateAsync(updatedTaskEntity))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _taskService.UpdateTaskAsync(taskId, taskUpdateDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldReturnFalse_WhenTaskNotFound()
        {
            // Arrange
            int taskId = 1;
            var taskUpdateDto = new TaskUpdateDto
            {
                Title = "Updated Task",
                Description = "Updated Description",
                TagIds = new List<int> { 1, 2 }
            };

            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId))
                               .ReturnsAsync((Domain.Entities.Task?)null);

            // Act
            var result = await _taskService.UpdateTaskAsync(taskId, taskUpdateDto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllTasksAsync_ShouldReturnAllTasks()
        {
            // Arrange
            var taskList = new List<Domain.Entities.Task>
            {
                new Domain.Entities.Task { Id = 1, Title = "Task 1" },
                new Domain.Entities.Task { Id = 2, Title = "Task 2" }
            };

            var taskGetDtoList = taskList.Select(task => new TaskGetDto { Id = task.Id, Title = task.Title }).ToList();

            _taskRepositoryMock.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(taskList);
            _taskMapperMock.Setup(mapper => mapper.MapToGetDto(It.IsAny<Domain.Entities.Task>()))
                           .Returns((Domain.Entities.Task task) => new TaskGetDto { Id = task.Id, Title = task.Title });

            // Act
            var result = await _taskService.GetAllTasksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
        
    }
}


