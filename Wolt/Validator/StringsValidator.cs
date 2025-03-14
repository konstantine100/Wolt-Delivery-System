using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class StringsValidator : AbstractValidator<Strings>
{
    public StringsValidator()
    {
        RuleFor(x => x.strings)
            .NotEmpty().WithMessage("Cannot be empty")
            .Length(2,50).WithMessage("Cannot be more than 50 characters");
    }
}