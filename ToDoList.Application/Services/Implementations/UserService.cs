using Serilog;
using ToDoList.Application.DTOs.User;
using ToDoList.Application.Services.Interfaces;
using ToDoList.Application.Services.Mapping;
using ToDoList.Domain.Interfaces;

namespace ToDoList.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;
    private readonly ILogger _logger;

    public UserService(IUserRepository userRepository, IUserMapper userMapper)
    {
        _userRepository = userRepository;
        _userMapper = userMapper;
        _logger = Log.ForContext<UserService>();
    }

    public async Task<UserGetDto?> GetUserByIdAsync(int id)
    {
        _logger.Information("Fetching user with ID {UserId}", id);
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            _logger.Warning("User with ID {UserId} not found", id);
            return null;
        }

        _logger.Information("User with ID {UserId} fetched successfully", id);
        return _userMapper.MapToGetDto(user);
    }

    public async Task<UserGetDto?> GetUserByEmailAsync(string email)
    {
        _logger.Information("Fetching user with email {Email}", email);
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            _logger.Warning("User with email {Email} not found", email);
            return null;
        }

        _logger.Information("User with email {Email} fetched successfully", email);
        return _userMapper.MapToGetDto(user);
    }

    public async Task<IEnumerable<UserGetDto>> GetAllUsersAsync()
    {
        _logger.Information("Fetching all users");
        var users = await _userRepository.GetAllAsync();
        _logger.Information("Fetched {UserCount} users", users.Count());
        return users.Select(_userMapper.MapToGetDto).ToList();
    }

    public async Task<UserGetDto?> CreateUserAsync(UserCreateDto userDto)
    {
        _logger.Information("Creating new user with email: {Email}", userDto.Email);
        var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
        if (existingUser != null)
        {
            _logger.Warning("User with email {Email} already exists", userDto.Email);
            return null;
        }
        
        var user = _userMapper.MapToEntity(userDto);
        await _userRepository.AddAsync(user);
        _logger.Information("User with ID {UserId} created successfully", user.Id);

        return _userMapper.MapToGetDto(user);
    }

    public async Task<bool> UpdateUserAsync(int id, UserUpdateDto userDto)
    {
        _logger.Information("Updating user with ID {UserId}", id);
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            _logger.Warning("User with ID {UserId} not found for update", id);
            return false;
        }

        existingUser = _userMapper.MapToEntity(userDto, existingUser);
        await _userRepository.UpdateAsync(existingUser);
        _logger.Information("User with ID {UserId} updated successfully", id);
        return true;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        _logger.Information("Deleting user with ID {UserId}", id);
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            _logger.Warning("User with ID {UserId} not found for deletion", id);
            return false;
        }

        await _userRepository.DeleteAsync(existingUser);
        _logger.Information("User with ID {UserId} deleted successfully", id);
        return true;
    }
}
