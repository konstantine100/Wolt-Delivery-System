using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class FoodValidator : AbstractValidator<Food>
{
    public FoodValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .Length(5, 50).WithMessage("Name must be between 5 and 50 characters");
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price cannot be empty")
            .Must(Price => !Price.ToString().Contains("e") && !Price.ToString().Contains("E"))
            .WithMessage("Price cannot contain scientific notation or invalid characters.")
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}