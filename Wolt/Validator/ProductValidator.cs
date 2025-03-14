using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .Length(2, 50).WithMessage("Name must be between 2 and 50 characters");
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price cannot be empty")
            .Must(Price => !Price.ToString().Contains("e") && !Price.ToString().Contains("E"))
            .WithMessage("Price cannot contain scientific notation or invalid characters.")
            .GreaterThan(0).WithMessage("Price must be greater than 0");
        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Quantity cannot be empty")
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}