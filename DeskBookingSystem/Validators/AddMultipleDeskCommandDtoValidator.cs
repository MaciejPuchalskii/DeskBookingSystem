using DeskBookingSystem.Dto;
using FluentValidation;

namespace DeskBookingSystem.Validators
{
    public class AddMultipleDeskCommandDtoValidator : AbstractValidator<AddMultipleDeskCommandDto>
    {
        public AddMultipleDeskCommandDtoValidator()
        {
            RuleFor(x => x.LocationId).GreaterThan(0).WithMessage("LocationId must be greater than 0.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
        }
    }
}