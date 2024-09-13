using System.ComponentModel.DataAnnotations;

namespace ToDoList.API.Contracts;
using Domain.Entities;

public record UpdateUserRequest(
    [Required] string Username,
    [Required] [EmailAddress] string Email,
    [Required] string PasswordHash,
    ICollection<int> TaskIds);