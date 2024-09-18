using ToDoList.Application.DTOs.User;
using ToDoList.Application.Services.Interfaces;
using ToDoList.Application.Services.Mapping;
using ToDoList.Domain.Interfaces;

namespace ToDoList.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;

    public UserService(IUserRepository userRepository, IUserMapper userMapper)
    {
        _userRepository = userRepository;
        _userMapper = userMapper;
    }


    public async Task<UserGetDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user != null ? _userMapper.MapToGetDto(user) : null;
    }

    public async Task<UserGetDto?> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user != null ? _userMapper.MapToGetDto(user) : null;
    }

    public async Task<IEnumerable<UserGetDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(_userMapper.MapToGetDto).ToList();
    }

    public async Task<UserGetDto?> CreateUserAsync(UserCreateDto userDto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
        if (existingUser != null)
            return null;
        
        var user = _userMapper.MapToEntity(userDto);
        await _userRepository.AddAsync(user);
        
        return _userMapper.MapToGetDto(user);
    }

    public async Task<bool> UpdateUserAsync(int id, UserUpdateDto userDto)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
            return false;

        existingUser = _userMapper.MapToEntity(userDto, existingUser);
        await _userRepository.UpdateAsync(existingUser);
        
        return true;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
            return false;

        await _userRepository.DeleteAsync(existingUser);
        return true;
    }
}