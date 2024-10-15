using DeskBookingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeskBookingSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly BookingContext _context;
        private readonly IConfiguration _configuration;

        public ReservationController(BookingContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPut("{reservationId}/changeReservationDate")]
        public IActionResult ChangeReservationDate(int deskId, int userId, int reservationId, DateTime newDate)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId && r.DeskId == deskId && r.UserId == userId);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            if ((reservation.ReservationDate - DateTime.Now).TotalHours <= 24)
            {
                return BadRequest("You can't change the reservation less than 24 hours before the reservation.");
            }

            bool isDeskReserved = _context.Reservations.Any(r => r.DeskId == deskId &&
               (r.ReservationDate < newDate.AddDays(reservation.HowManyDays) &&
                r.ReservationDate.AddDays(r.HowManyDays) > newDate));

            if (isDeskReserved)
            {
                return BadRequest("The new desk is not available for the selected time period.");
            }

            reservation.ReservationDate = newDate;
            _context.SaveChanges();

            return Ok("Reservation changed successfully.");
        }

        [HttpPut("{reservationId}/changeReservationDesk/{deskId}")]
        public IActionResult ChangeReservationDesk(int reservationId, int newDeskId)
        {
            var reservation = _context.Reservations.Include(r => r.Desk).FirstOrDefault(r => r.Id == reservationId);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            if ((reservation.ReservationDate - DateTime.Now).TotalHours <= 24)
            {
                return BadRequest("You can't change the desk less than 24 hours before the reservation.");
            }

            var newDesk = _context.Desks
                .FirstOrDefault(d => d.Id == newDeskId && d.IsAvailable &&
                    !_context.Reservations.Any(r => r.DeskId == newDeskId &&
                        (r.ReservationDate < reservation.ReservationDate.AddDays(reservation.HowManyDays) &&
                         r.ReservationDate.AddDays(r.HowManyDays) > reservation.ReservationDate)));

            if (newDesk == null)
            {
                return BadRequest("The new desk is not available for the selected time period.");
            }

            reservation.DeskId = newDeskId;
            _context.SaveChanges();

            return Ok("Desk changed successfully.");
        }
    }
}