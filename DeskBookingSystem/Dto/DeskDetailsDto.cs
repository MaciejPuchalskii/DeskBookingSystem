namespace DeskBookingSystem.Dto
{
    public class DeskDetailsDto
    {
        public int Id { get; set; }
        public bool IsOperational { get; set; }
        public string LocationName { get; set; }
        public List<ReservationDto> Reservations { get; set; }
    }
}