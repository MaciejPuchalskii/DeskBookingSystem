using DeskBookingSystem.Dto;

namespace DeskBookingSystem.Services
{
    public interface IDeskService
    {
        AddDeskResponseDto Add(AddDeskCommandDto addDeskCommandDto);

        RemoveDeskResponseDto Remove(RemoveDeskCommandDto removeDeskCommandDto);

        ChangeDeskAvailabiltyResponseDto ChangeDeskAvailability(ChangeDeskAvailabiltyCommandDto setDeskCommandDto);

        GetDeskDetailsResponseDto GetDeskDetails(GetDeskDetailsQueryDto getDeskDetailsQueryDto);
    }
}