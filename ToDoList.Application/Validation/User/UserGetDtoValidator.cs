using FluentValidation;
using ToDoList.Application.DTOs.User;

namespace ToDoList.Application.Validation.User;

public class UserGetDtoValidator : AbstractValidator<UserGetDto>
{
    public UserGetDtoValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(2, 50).WithMessage("Username must be between 2 and 50 characters.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");
    }
}