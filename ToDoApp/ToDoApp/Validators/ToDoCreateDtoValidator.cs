using FluentValidation;
using ToDoApp.Dtos;

namespace ToDoApp.Validators;

public class ToDoCreateDtoValidator : AbstractValidator<ToDoCreateDto>
{
    public ToDoCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500).When(x => x.Description != null);

        RuleFor(x => x.ExpiryDate)
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("Expiry date must be in the future.");
    }
}
