using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class IngridientsValidator : AbstractValidator<Ingridients>
{
    public IngridientsValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .Length(2,50).WithMessage("Name must be between 2 and 50 characters.");
        RuleFor(x => x.Calories)
            .NotEmpty().WithMessage("Calories cannot be empty.")
            .Must(Calories => !Calories.ToString().Contains("e") && !Calories.ToString().Contains("E"))
            .WithMessage("Price cannot contain scientific notation or invalid characters.")
            .GreaterThanOrEqualTo(0).WithMessage("Calories must be greater than or equal to zero.");
        RuleFor(x => x.AdditionalPrice)
            .NotEmpty().WithMessage("Additional price cannot be empty.")
            .Must(Calories => !Calories.ToString().Contains("e") && !Calories.ToString().Contains("E"))
            .WithMessage("Price cannot contain scientific notation or invalid characters.")
            .GreaterThanOrEqualTo(0).WithMessage("Additional price must be greater than or equal to zero.");
        
    }
}