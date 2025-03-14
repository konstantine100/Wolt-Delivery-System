using FluentValidation;
using Wolt.Models;

namespace Wolt.Validator;

public class ScheduleValidator : AbstractValidator<Schedule>
{
    public ScheduleValidator()
    {
        RuleFor(x => x.MondayOpenTime)
            .NotEmpty().WithMessage("Monday open time cannot be empty");
            
        RuleFor(x => x.MondayCloseTime)
            .NotEmpty().WithMessage("Monday close time cannot be empty");

        RuleFor(x => x.TuesdayOpenTime)
            .NotEmpty().WithMessage("Tuesday open time cannot be empty");

        RuleFor(x => x.TuesdayCloseTime)
            .NotEmpty().WithMessage("Tuesday close time cannot be empty");

        RuleFor(x => x.WednesdayOpenTime)
            .NotEmpty().WithMessage("Wednesday open time cannot be empty");
        RuleFor(x => x.WednesdayCloseTime)
            .NotEmpty().WithMessage("Wednesday close time cannot be empty");
        
        RuleFor(x => x.ThursdayOpenTime)
            .NotEmpty().WithMessage("Thursday open time cannot be empty");
        RuleFor(x => x.ThursdayCloseTime)
            .NotEmpty().WithMessage("Thursday close time cannot be empty");
        
        RuleFor(x => x.FridayOpenTime)
            .NotEmpty().WithMessage("Friday open time cannot be empty");
        RuleFor(x => x.FridayCloseTime)
            .NotEmpty().WithMessage("Friday close time cannot be empty");
        
        RuleFor(x => x.SaturdayOpenTime)
            .NotEmpty().WithMessage("Saturday open time cannot be empty");
        RuleFor(x => x.SaturdayCloseTime)
            .NotEmpty().WithMessage("Saturday close time cannot be empty");
        
        RuleFor(x => x.SundayOpenTime)
            .NotEmpty().WithMessage("Sunday open time cannot be empty");
        RuleFor(x => x.SundayCloseTime)
            .NotEmpty().WithMessage("Sunday close time cannot be empty");
    }
}