using System.Text;
using ToDoList.Application.DTOs.User;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services.Mapping;

public class UserMapper : IUserMapper
{
    public UserGetDto MapToGetDto(User user)
    {
        return new UserGetDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            PasswordHash = user.PasswordHash
        };
    }

    public User MapToEntity(UserCreateDto userCreateDto)
    {
        return new User
        {
            Email = userCreateDto.Email,
            Username = userCreateDto.Username,
            PasswordHash = HashPassword(userCreateDto.Password)
        };
    }

    public User MapToEntity(UserUpdateDto userUpdateDto, User user)
    {
        user.Email = userUpdateDto.Email;
        user.Username = userUpdateDto.Username;
        if (!string.IsNullOrEmpty(userUpdateDto.Password))
        {
            user.PasswordHash = HashPassword(userUpdateDto.Password);
        }

        return user;
    }
    
    private string HashPassword(string password)
    {
        // use hash algorithm
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(password)); 
    }
}