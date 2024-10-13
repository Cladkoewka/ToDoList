using FluentValidation;
using ToDoList.Application.DTOs.User;

namespace ToDoList.Application.Validation.User;

public class UserWithTasksDtoValidator : AbstractValidator<UserWithTasksDto>
{
    public UserWithTasksDtoValidator()
    {

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");
        
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 20).WithMessage("Name must be between 2 and 20 characters.");
    }
}