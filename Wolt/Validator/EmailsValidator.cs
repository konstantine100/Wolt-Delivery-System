using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class EmailsValidator : AbstractValidator<Emails>
{
    public EmailsValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Email is required");
    }
}