namespace DeskBookingSystem.Dto
{
    public class ChangeReservationDateCommandDto
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public DateTime NewDate { get; set; }
        public int DaysCount { get; set; }
    }
}