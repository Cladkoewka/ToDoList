using FluentAssertions;
using Moq;
using AutoFixture;
using ToDoList.Application.DTOs.User;
using ToDoList.Application.Services.Implementations;
using ToDoList.Application.Services.Mapping;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Interfaces;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUserMapper> _userMapperMock;
        private readonly UserService _userService;
        private readonly IFixture _fixture;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userMapperMock = new Mock<IUserMapper>();
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());


            _userService = new UserService(
                _userRepositoryMock.Object,
                _userMapperMock.Object
            );
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            int userId = _fixture.Create<int>();
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            string email = _fixture.Create<string>();
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetUserByEmailAsync(email);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var userList = _fixture.CreateMany<User>(2).ToList();

            _userRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(userList);
            _userMapperMock.Setup(mapper => mapper.MapToGetDto(It.IsAny<User>()))
                .Returns((User user) => new UserGetDto { Id = user.Id, Email = user.Email });

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnUser_WhenUserCreated()
        {
            // Arrange
            var userCreateDto = new UserCreateDto
            {
                Email = "test@example.com" 
            };
    
            var existingUser = (User?)null; // No existing user
            var userEntity = _fixture.Build<User>()
                .Without(u => u.Id) 
                .With(u => u.Email, userCreateDto.Email) 
                .Create();
            var userGetDto = _fixture.Build<UserGetDto>()
                .With(dto => dto.Email, userCreateDto.Email) 
                .Create();

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(userCreateDto.Email))
                .ReturnsAsync(existingUser);
            _userMapperMock.Setup(mapper => mapper.MapToEntity(userCreateDto))
                .Returns(userEntity);
            _userRepositoryMock.Setup(repo => repo.AddAsync(userEntity))
                .Returns(Task.CompletedTask);
            _userMapperMock.Setup(mapper => mapper.MapToGetDto(userEntity))
                .Returns(userGetDto);

            // Act
            var result = await _userService.CreateUserAsync(userCreateDto);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(userCreateDto.Email);
        }


        [Fact]
        public async Task CreateUserAsync_ShouldReturnNull_WhenUserAlreadyExists()
        {
            // Arrange
            var userCreateDto = _fixture.Create<UserCreateDto>();
            var existingUser = _fixture.Create<User>(); // Simulate existing user
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(userCreateDto.Email))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _userService.CreateUserAsync(userCreateDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnTrue_WhenUserUpdated()
        {
            // Arrange
            int userId = _fixture.Create<int>();
            var userUpdateDto = _fixture.Create<UserUpdateDto>();
            var existingUser = _fixture.Build<User>()
                                        .With(u => u.Id, userId)
                                        .Create();

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(existingUser);
            _userMapperMock.Setup(mapper => mapper.MapToEntity(userUpdateDto, existingUser))
                .Returns(existingUser);
            _userRepositoryMock.Setup(repo => repo.UpdateAsync(existingUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUserAsync(userId, userUpdateDto);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            int userId = _fixture.Create<int>();
            var userUpdateDto = _fixture.Create<UserUpdateDto>();

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.UpdateUserAsync(userId, userUpdateDto);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserDeleted()
        {
            // Arrange
            int userId = _fixture.Create<int>();
            var existingUser = _fixture.Build<User>()
                                        .With(u => u.Id, userId)
                                        .Create();

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(repo => repo.DeleteAsync(existingUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.DeleteUserAsync(userId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            int userId = _fixture.Create<int>();
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.DeleteUserAsync(userId);

            // Assert
            result.Should().BeFalse();
        }
    }
}
