using FluentValidation;
using ToDoList.Application.DTOs.User;
using ToDoList.Application.Validation.Task;

namespace ToDoList.Application.Validation.User;

public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 20).WithMessage("Name must be between 2 and 20 characters.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}