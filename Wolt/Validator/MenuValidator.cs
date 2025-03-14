using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class MenuValidator : AbstractValidator<Menu>
{
    public MenuValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .Length(5, 100).WithMessage("Description must be between 5 and 100 characters.");
    }
}