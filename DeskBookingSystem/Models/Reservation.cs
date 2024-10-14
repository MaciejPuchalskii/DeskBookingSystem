using System.ComponentModel.DataAnnotations;

namespace DeskBookingSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime ReservationDate { get; set; }

        [Range(1, 7, ErrorMessage = "Reservation cannot exceed 7 days.")]
        public int HowManyDays { get; set; }

        public int DeskId { get; set; }
        public Desk Desk { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}