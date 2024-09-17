using FluentValidation;
using ToDoList.Application.DTOs.Task;

namespace ToDoList.Application.Validation.Task;

public class TaskGetDtoValidator : AbstractValidator<TaskGetDto>
{
    public TaskGetDtoValidator()
    {
        RuleFor(task => task.Title)
            .NotEmpty().WithMessage("Task title is required.")
            .MaximumLength(100).WithMessage("Task title must less 100 characters.");

    }
}