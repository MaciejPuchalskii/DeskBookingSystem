namespace DeskBookingSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime ReservationDate { get; set; }

        public int HowManyDays { get; set; }

        public int DeskId { get; set; }
        public Desk Desk { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}