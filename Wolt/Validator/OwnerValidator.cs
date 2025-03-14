using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class OwnerValidator : AbstractValidator<Owner>
{
    public OwnerValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty")
            .Length(2,50).WithMessage("First name must be between 2 and 50 characters");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty")
            .Length(2,50).WithMessage("Last name must be between 2 and 50 characters");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Invalid email address");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .Length(8,20).WithMessage("Password must be between 6 and 20 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone cannot be empty.")
            .Matches(@"5\d{2}-?\d{3}-?\d{3}").WithMessage("It must be in Georgian number format.");
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth cannot be empty.")
            .LessThan(DateTime.Today).WithMessage("Date of birth must be greater than today's date.");
    }
}