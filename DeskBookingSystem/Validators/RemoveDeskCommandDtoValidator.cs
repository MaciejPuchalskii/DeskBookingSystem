using DeskBookingSystem.Dto;
using FluentValidation;

namespace DeskBookingSystem.Validators
{
    public class RemoveDeskCommandDtoValidator : AbstractValidator<RemoveDeskCommandDto>
    {
        public RemoveDeskCommandDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}