namespace DeskBookingSystem.Dto
{
    public class GetDeskDetailsResponseDto
    {
        public int Id { get; set; }
        public int LocatioId { get; set; }
        public bool IsOperational { get; set; }

        public List<ReservationDto> Reservations { get; set; }
    }
}