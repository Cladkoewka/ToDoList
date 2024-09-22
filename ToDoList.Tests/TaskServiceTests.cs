using Moq;
using AutoFixture;
using ToDoList.Application.DTOs.Task;
using ToDoList.Application.Services.Implementations;
using ToDoList.Application.Services.Mapping;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Interfaces;
using Xunit;
using FluentAssertions;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<ITaskMapper> _taskMapperMock;
        private readonly TaskService _taskService;
        private readonly Fixture _fixture;

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
            _fixture = new Fixture(); 
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskNotFound()
        {
            // Arrange
            int taskId = _fixture.Create<int>();
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId))
                               .ReturnsAsync((Domain.Entities.Task?)null);

            // Act
            var result = await _taskService.GetTaskByIdAsync(taskId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldReturnFalse_WhenTaskNotFound()
        {
            // Arrange
            int taskId = _fixture.Create<int>();
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId))
                               .ReturnsAsync((Domain.Entities.Task?)null);

            // Act
            var result = await _taskService.DeleteTaskAsync(taskId);

            // Assert
            result.Should().BeFalse();
        }
        
        [Fact]
        public async Task CreateTaskAsync_ShouldReturnTask_WhenTaskCreated()
        {
            // Arrange
            var taskCreateDto = _fixture.Create<TaskCreateDto>();

            var tags = new List<Tag> { new Tag { Id = 1 }, new Tag { Id = 2 } };
            var taskEntity = _fixture.Build<Domain.Entities.Task>()
                                     .With(t => t.Id, 1)
                                     .With(t => t.Title, taskCreateDto.Title)
                                     .With(t => t.Description, taskCreateDto.Description)
                                     .Create();
            var taskGetDto = _fixture.Build<TaskGetDto>()
                                      .With(t => t.Id, 1)
                                      .With(t => t.Title, taskCreateDto.Title)
                                      .With(t => t.Description, taskCreateDto.Description)
                                      .Create();

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
            result.Should().NotBeNull();
            result.Title.Should().Be(taskCreateDto.Title);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldReturnTrue_WhenTaskUpdated()
        {
            // Arrange
            int taskId = 1;
            var taskUpdateDto = _fixture.Create<TaskUpdateDto>();

            var existingTask = _fixture.Build<Domain.Entities.Task>()
                                       .With(t => t.Id, taskId)
                                       .With(t => t.Title, "Old Task")
                                       .With(t => t.Description, "Old Description")
                                       .Create();
            var tags = new List<Tag> { new Tag { Id = 1 }, new Tag { Id = 2 } };
            var updatedTaskEntity = _fixture.Build<Domain.Entities.Task>()
                                            .With(t => t.Id, taskId)
                                            .With(t => t.Title, taskUpdateDto.Title)
                                            .With(t => t.Description, taskUpdateDto.Description)
                                            .Create();

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
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldReturnFalse_WhenTaskNotFound()
        {
            // Arrange
            int taskId = 1;
            var taskUpdateDto = _fixture.Create<TaskUpdateDto>();

            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId))
                               .ReturnsAsync((Domain.Entities.Task?)null);

            // Act
            var result = await _taskService.UpdateTaskAsync(taskId, taskUpdateDto);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllTasksAsync_ShouldReturnAllTasks()
        {
            // Arrange
            var taskList = _fixture.CreateMany<Domain.Entities.Task>(2).ToList();

            var taskGetDtoList = taskList.Select(task => new TaskGetDto { Id = task.Id, Title = task.Title }).ToList();

            _taskRepositoryMock.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(taskList);
            _taskMapperMock.Setup(mapper => mapper.MapToGetDto(It.IsAny<Domain.Entities.Task>()))
                           .Returns((Domain.Entities.Task task) => new TaskGetDto { Id = task.Id, Title = task.Title });

            // Act
            var result = await _taskService.GetAllTasksAsync();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }
    }
}
