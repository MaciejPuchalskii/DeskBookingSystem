namespace DeskBookingSystem.Dto
{
    public class ChangeReservationDateResponseDto
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public int DeskId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}