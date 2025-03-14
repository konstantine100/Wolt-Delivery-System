using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class MenuCategoryValidator : AbstractValidator<MenuCategory>
{
    public MenuCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .Length(5,50).WithMessage("Name must be between 5 and 50 characters.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .Length(5,100).WithMessage("Description must be between 5 and 50 characters.");
    }
}