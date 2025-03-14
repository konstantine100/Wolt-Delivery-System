using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class StoreCategoryValidator : AbstractValidator<StoreCategory>
{
    public StoreCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .Length(5,50).WithMessage("Name must be between 5 and 50 characters.");
        
    }
}