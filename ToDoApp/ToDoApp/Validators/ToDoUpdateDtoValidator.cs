using FluentValidation;
using ToDoApp.Dtos;

namespace ToDoApp.Validators;

public class ToDoUpdateDtoValidator : AbstractValidator<ToDoUpdateDto>
{
    public ToDoUpdateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500).When(x => x.Description != null);

        RuleFor(x => x.ExpiryDate)
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("Expiry date must be in the future.");

        RuleFor(x => x.PercentComplete)
            .InclusiveBetween(0, 100);
    }
}
