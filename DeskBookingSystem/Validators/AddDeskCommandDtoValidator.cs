using DeskBookingSystem.Dto;
using FluentValidation;

namespace DeskBookingSystem.Validators
{
    public class AddDeskCommandDtoValidator : AbstractValidator<AddDeskCommandDto>
    {
        public AddDeskCommandDtoValidator()
        {
            RuleFor(x => x.LocationId).GreaterThan(0).WithMessage("LocationId must be greater than 0.");
        }
    }
}