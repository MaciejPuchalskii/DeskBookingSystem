namespace DeskBookingSystem.Dto
{
    public class ReserveDeskResponseDto
    {
        public int ReservationId { get; set; }
        public int DeskId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}