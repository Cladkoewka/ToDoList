using System.ComponentModel.DataAnnotations;

namespace ToDoList.API.Contracts;

public record AddUserRequest(
    [Required] string Username,
    [Required] [EmailAddress] string Email,
    [Required] string PasswordHash,
    ICollection<int> TaskIds);