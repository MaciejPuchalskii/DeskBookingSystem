namespace DeskBookingSystem.Data.Models
{
    public class Desk
    {
        public int Id { get; set; }
        public bool IsOperational { get; set; }

        public int LocationId { get; set; }

        public Location Location { get; set; }

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}