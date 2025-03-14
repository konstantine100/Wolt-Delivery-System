using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class ProdCategoryValidator : AbstractValidator<ProdCategory>
{
    public ProdCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .Length(2, 50).WithMessage("Name must be between 2 and 50 characters");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty")
            .Length(2, 50).WithMessage("Description must be between 2 and 50 characters");
    }
}