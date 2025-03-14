using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class FastPasswordValidator : AbstractValidator<FastPassword>
{
    public FastPasswordValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .Length(8,20).WithMessage("Password must be between 6 and 20 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
    }
}