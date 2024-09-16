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
            Email = user.Email
        };
    }

    public User MapToEntity(UserCreateDto userCreateDto)
    {
        return new User
        {
            Username = userCreateDto.Username,
            Email = userCreateDto.Email,
            PasswordHash = userCreateDto.PasswordHash
        };
    }

    public User MapToEntity(UserUpdateDto userUpdateDto, User user)
    {
        user.Username = userUpdateDto.Username;
        user.Email = userUpdateDto.Email;
        if (!string.IsNullOrEmpty(userUpdateDto.PasswordHash))
        {
            user.PasswordHash = userUpdateDto.PasswordHash;
        }

        return user;
    }
}