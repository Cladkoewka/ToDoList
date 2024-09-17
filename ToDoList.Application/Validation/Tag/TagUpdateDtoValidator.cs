using FluentValidation;
using ToDoList.Application.DTOs.Tag;

namespace ToDoList.Application.Validation.Tag;

public class TagUpdateDtoValidator : AbstractValidator<TagUpdateDto>
{
    public TagUpdateDtoValidator()
    {
        RuleFor(tag => tag.Name)
            .NotEmpty().WithMessage("Tag name is required.")
            .Length(2, 50).WithMessage("Tag name must be between 2 and 50 characters.");
    }
}