using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class StoreScheduleValidator : AbstractValidator<StoreSchedule>
{
    public StoreScheduleValidator()
    {
        RuleFor(x => x.MondayOpenTime)
            .NotEmpty().WithMessage("Monday open time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Monday open time must be greater than zero.");
        RuleFor(x => x.MondayCloseTime)
            .NotEmpty().WithMessage("Monday close time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Monday close time must be greater than zero.");
        
        RuleFor(x => x.TuesdayOpenTime)
            .NotEmpty().WithMessage("Tuesday open time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Tuesday open time must be greater than zero.");
        RuleFor(x => x.TuesdayCloseTime)
            .NotEmpty().WithMessage("Tuesday close time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Tuesday close time must be greater than zero.");
        
        RuleFor(x => x.WednesdayOpenTime)
            .NotEmpty().WithMessage("Wednesday open time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Wednesday open time must be greater than zero.");
        RuleFor(x => x.WednesdayCloseTime)
            .NotEmpty().WithMessage("Wednesday close time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Wednesday close time must be greater than zero.");
        
        RuleFor(x => x.ThursdayOpenTime)
            .NotEmpty().WithMessage("Thursday open time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Thursday open time must be greater than zero.");
        RuleFor(x => x.ThursdayCloseTime)
            .NotEmpty().WithMessage("Thursday close time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Thursday close time must be greater than zero.");
        
        RuleFor(x => x.FridayOpenTime)
            .NotEmpty().WithMessage("Friday open time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Friday open time must be greater than zero.");
        RuleFor(x => x.FridayCloseTime)
            .NotEmpty().WithMessage("Friday close time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Friday close time must be greater than zero.");
        
        RuleFor(x => x.SaturdayOpenTime)
            .NotEmpty().WithMessage("Saturday open time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Saturday open time must be greater than zero.");
        RuleFor(x => x.SaturdayCloseTime)
            .NotEmpty().WithMessage("Saturday close time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Saturday close time must be greater than zero.");
        
        RuleFor(x => x.SundayOpenTime)
            .NotEmpty().WithMessage("Sunday open time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Sunday open time must be greater than zero.");
        RuleFor(x => x.SundayCloseTime)
            .NotEmpty().WithMessage("Sunday close time cannot be empty")
            .GreaterThan(TimeSpan.Zero).WithMessage("Sunday close time must be greater than zero.");
    }
}