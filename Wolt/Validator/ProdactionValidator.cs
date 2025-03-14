using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class ProdactionValidator : AbstractValidator<Prodaction>
{
    public ProdactionValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty")
            .Length(2, 50).WithMessage("Description must be between 2 and 50 characters");
    }
}