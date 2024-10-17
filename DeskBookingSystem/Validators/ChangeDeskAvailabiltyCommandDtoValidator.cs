using DeskBookingSystem.Dto;
using FluentValidation;

namespace DeskBookingSystem.Validators
{
    public class ChangeDeskAvailabiltyCommandDtoValidator : AbstractValidator<ChangeDeskAvailabiltyCommandDto>
    {
        public ChangeDeskAvailabiltyCommandDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}