namespace DeskBookingSystem.Dto
{
    public class GetDesksByAvailabilityResponseDto
    {
        public List<DeskDetailsDto> Desks { get; set; } = new List<DeskDetailsDto>();
    }
}