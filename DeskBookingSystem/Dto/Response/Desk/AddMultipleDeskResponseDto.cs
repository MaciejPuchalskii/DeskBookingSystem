namespace DeskBookingSystem.Dto
{
    public class AddMultipleDeskResponseDto
    {
        public List<AddDeskResponseDto> AddedDesks { get; set; } = new List<AddDeskResponseDto>();
        public int TotalAdded { get; set; }
    }
}