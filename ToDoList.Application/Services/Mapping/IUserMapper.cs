using ToDoList.Application.DTOs.User;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services.Mapping;

public interface IUserMapper
{
    UserGetDto MapToGetDto(User user);
    User MapToEntity(UserCreateDto userCreateDto);
    User MapToEntity(UserUpdateDto userUpdateDto, User user);
}