using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class CustomerDetailsValidator : AbstractValidator<CustomerDetails>
{
    public CustomerDetailsValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address cannot be empty.")
            .Length(10,100).WithMessage("Address length must be between 10 and 100 characters.");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone cannot be empty.")
            .Matches(@"5\d{2}-?\d{3}-?\d{3}").WithMessage("It must be in Georgian number format.");
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth cannot be empty.")
            .LessThan(DateTime.Today).WithMessage("Date of birth must be greater than today's date.");
    }
}