namespace DeskBookingSystem.Models
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime ReservationDate { get; set; }
        public int HowManyDays { get; set; }
        public int UserId { get; set; }
    }
}