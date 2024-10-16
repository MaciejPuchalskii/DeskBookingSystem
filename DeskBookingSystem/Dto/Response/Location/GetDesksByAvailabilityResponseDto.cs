using DeskBookingSystem.Dto.Response.Location;

namespace DeskBookingSystem.Dto
{
    public class GetDesksByAvailabilityResponseDto
    {
        public List<GetDeskByAvailabilityResponseDto> Desks { get; set; } = new List<GetDeskByAvailabilityResponseDto>();
    }
}