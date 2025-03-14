using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class FoodChainValidator : AbstractValidator<FoodChain>
{
    public FoodChainValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .Length(5,50).WithMessage("Name must be between 5 and 50 characters.");
        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City cannot be empty.")
            .Length(5,50).WithMessage("City must be between 5 and 50 characters.");
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address cannot be empty.")
            .Length(5,50).WithMessage("Address must be between 5 and 50 characters.");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone cannot be empty.")
            .Matches(@"5\d{2}-?\d{3}-?\d{3}").WithMessage("It must be in Georgian number format.");
        RuleFor(x => x.OrderFee)
            .NotEmpty().WithMessage("Order Fee cannot be empty.")
            .GreaterThan(0).WithMessage("Order Fee must be greater than zero.")
            .Must(OrderFee => !OrderFee.ToString().Contains("e") && !OrderFee.ToString().Contains("E"))
            .WithMessage("Price cannot contain scientific notation or invalid characters.");
        RuleFor(x => x.OrderTime)
            .NotEmpty().WithMessage("Order Time cannot be empty.")
            .GreaterThan(TimeSpan.Zero).WithMessage("Order Time must be greater than zero.");
    }
}