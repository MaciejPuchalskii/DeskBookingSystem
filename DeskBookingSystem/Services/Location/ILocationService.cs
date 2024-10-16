using DeskBookingSystem.Dto;

namespace DeskBookingSystem.Services
{
    public interface ILocationService
    {
        void ExistLocation(int locationId);

        AddLocationResponseDto Add(AddLocationCommandDto addLocationCommandDto);

        RemoveLocationResponseDto Remove(RemoveLocationCommandDto removeLocationCommandDto);

        GetDesksFromLocationResponseDto GetDesks(GetDesksFromLocationQueryDto getDesksFromLocationQueryDto);

        GetDesksByAvailabilityResponseDto GetDesksByAvailability(GetDesksByAvailabilityQueryDto getDesksByAvailabilityQueryDto);
    }
}