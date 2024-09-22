using Moq;
using ToDoList.Application.DTOs.User;
using ToDoList.Application.Services.Implementations;
using ToDoList.Application.Services.Mapping;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Interfaces;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUserMapper> _userMapperMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userMapperMock = new Mock<IUserMapper>();

        _userService = new UserService(
            _userRepositoryMock.Object,
            _userMapperMock.Object
        );
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        int userId = 1;
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        string email = "test@example.com";
        _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetUserByEmailAsync(email);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var userList = new List<User>
        {
            new User { Id = 1, Email = "user1@example.com" },
            new User { Id = 2, Email = "user2@example.com" }
        };

        var userGetDtoList = userList.Select(user => new UserGetDto { Id = user.Id, Email = user.Email }).ToList();

        _userRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(userList);
        _userMapperMock.Setup(mapper => mapper.MapToGetDto(It.IsAny<User>()))
            .Returns((User user) => new UserGetDto { Id = user.Id, Email = user.Email });

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnUser_WhenUserCreated()
    {
        // Arrange
        var userCreateDto = new UserCreateDto
        {
            Email = "newuser@example.com"
        };

        var existingUser = (User?)null; // No existing user
        var userEntity = new User { Id = 1, Email = "newuser@example.com" };
        var userGetDto = new UserGetDto { Id = 1, Email = "newuser@example.com" };

        _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(userCreateDto.Email))
            .ReturnsAsync(existingUser);
        _userMapperMock.Setup(mapper => mapper.MapToEntity(userCreateDto))
            .Returns(userEntity);
        _userRepositoryMock.Setup(repo => repo.AddAsync(userEntity));
        _userMapperMock.Setup(mapper => mapper.MapToGetDto(userEntity))
            .Returns(userGetDto);

        // Act
        var result = await _userService.CreateUserAsync(userCreateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("newuser@example.com", result.Email);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnNull_WhenUserAlreadyExists()
    {
        // Arrange
        var userCreateDto = new UserCreateDto
        {
            Email = "existinguser@example.com"
        };

        var existingUser = new User { Id = 1, Email = "existinguser@example.com" };
        _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(userCreateDto.Email))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _userService.CreateUserAsync(userCreateDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnTrue_WhenUserUpdated()
    {
        // Arrange
        int userId = 1;
        var userUpdateDto = new UserUpdateDto
        {
            Email = "updateduser@example.com"
        };

        var existingUser = new User { Id = userId, Email = "olduser@example.com" };

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(existingUser);
        _userMapperMock.Setup(mapper => mapper.MapToEntity(userUpdateDto, existingUser))
            .Returns(existingUser);
        _userRepositoryMock.Setup(repo => repo.UpdateAsync(existingUser))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _userService.UpdateUserAsync(userId, userUpdateDto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        // Arrange
        int userId = 1;
        var userUpdateDto = new UserUpdateDto
        {
            Email = "updateduser@example.com"
        };

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.UpdateUserAsync(userId, userUpdateDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserDeleted()
    {
        // Arrange
        int userId = 1;
        var existingUser = new User { Id = userId, Email = "userToDelete@example.com" };

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(repo => repo.DeleteAsync(existingUser))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _userService.DeleteUserAsync(userId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        // Arrange
        int userId = 1;
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.DeleteUserAsync(userId);

        // Assert
        Assert.False(result);
    }
}

