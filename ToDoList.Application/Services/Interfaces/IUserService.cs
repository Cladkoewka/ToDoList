using ToDoList.Application.DTOs.User;

namespace ToDoList.Application.Services.Interfaces;

public interface IUserService
{
    Task<UserGetDto?> GetUserByIdAsync(int id);
    Task<UserGetDto?> GetUserByEmailAsync(string email);
    Task<IEnumerable<UserGetDto>> GetAllUsersAsync();
    Task<UserGetDto?> CreateUserAsync(UserCreateDto userDto);
    Task<bool> UpdateUserAsync(int id, UserUpdateDto userDto);
    Task<bool> DeleteUserAsync(int id);
}