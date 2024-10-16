namespace DeskBookingSystem.Dto
{
    public class GetDesksFromLocationResponseDto
    {
        public int LocationId { get; set; }
        public List<DeskDetailsDto> Desks { get; set; } = new List<DeskDetailsDto>();
    }
}