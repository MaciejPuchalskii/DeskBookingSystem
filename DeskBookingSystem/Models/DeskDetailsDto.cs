namespace DeskBookingSystem.Models
{
    public class DeskDetailsDto
    {
        public int Id { get; set; }
        public bool IsAvailable { get; set; }
        public string LocationName { get; set; }
        public List<ReservationDto> Reservations { get; set; }
    }
}