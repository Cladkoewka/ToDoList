using FluentValidation;
using ToDoList.Application.DTOs.Task;

namespace ToDoList.Application.Validation.Task;

public class TaskUpdateDtoValidator : AbstractValidator<TaskUpdateDto>
{
    public TaskUpdateDtoValidator()
    {
        RuleFor(task => task.Title)
            .NotEmpty().WithMessage("Task title is required.")
            .MaximumLength(100).WithMessage("Task title must less 100 characters.");

        RuleFor(task => task.Description)
            .MaximumLength(500).WithMessage("Task description must be at most 500 characters long.");

    }
}