using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class DatesValidator : AbstractValidator<Dates>
{
    public DatesValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date of birth cannot be empty.")
            .LessThan(DateTime.Today).WithMessage("Date of birth must be greater than today's date.");
    }
}