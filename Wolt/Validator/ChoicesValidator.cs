using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class ChoicesValidator : AbstractValidator<Choices>
{
    public ChoicesValidator(int length)
    {
        RuleFor(x => x.Choice)
            .NotEmpty().WithMessage("Choice Input is required")
            .Matches(@"^\d+$").WithMessage("Choice Input is invalid");
            
        RuleFor(x => int.Parse(x.Choice))
            .GreaterThan(0).WithMessage("Choice Input must be greater than 0")
            .LessThanOrEqualTo(length).WithMessage($"Choice Input must be between 1 and {length}");

    }
}