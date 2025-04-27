using FluentValidation.TestHelper;
using ToDoApp.Dtos;
using ToDoApp.Validators;

namespace ToDoApp.Tests.Validators;

public class ToDoCreateDtoValidatorTests
{
    private readonly ToDoCreateDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var dto = new ToDoCreateDto { Title = "", ExpiryDate = DateTime.UtcNow.AddDays(1) };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_ExpiryDate_Is_In_The_Past()
    {
        var dto = new ToDoCreateDto { Title = "Task", ExpiryDate = DateTime.UtcNow.AddDays(-1) };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.ExpiryDate);
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Valid()
    {
        var dto = new ToDoCreateDto { Title = "Task", ExpiryDate = DateTime.UtcNow.AddDays(2) };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
