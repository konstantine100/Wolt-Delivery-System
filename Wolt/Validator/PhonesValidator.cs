using FluentValidation;

namespace Wolt.Validator;

public class PhonesValidator : AbstractValidator<Phones>
{
    public PhonesValidator()
    {
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone cannot be empty.")
            .Matches(@"5\d{2}-?\d{3}-?\d{3}").WithMessage("It must be in Georgian number format.");
        
    }
}