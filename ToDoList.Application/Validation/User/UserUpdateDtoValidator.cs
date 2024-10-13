using FluentValidation;
using ToDoList.Application.DTOs.User;

namespace ToDoList.Application.Validation.User;

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");
        
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 20).WithMessage("Name must be between 2 and 20 characters.");

        RuleFor(user => user.Password)
            .MinimumLength(6).When(user => !string.IsNullOrEmpty(user.Password))
            .WithMessage("Password must be at least 6 characters long if provided.");
    }
}