using DeskBookingSystem.Data;
using DeskBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DeskBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeskController : Controller
    {
        private readonly BookingContext _context;

        public DeskController(BookingContext context)
        {
            _context = context;
        }

        [HttpPost("{deskId}/reserve")]
        public IActionResult ReserveDesk(int deskId, int userId, DateTime reservationDate, int howManyDays)
        {
            var desk = _context.Desks.Include(d => d.Reservations).FirstOrDefault(d => d.Id == deskId);
            if (desk == null)
            {
                return NotFound("Desk not found.");
            }

            if (!desk.IsAvailable)
            {
                return BadRequest("Desk is not available for reservation.");
            }
            var ifDeskHasAnyConflictingReservations = _context.Reservations.Any(r => r.DeskId == deskId &&
                  (r.ReservationDate < reservationDate.AddDays(howManyDays) &&
                   r.ReservationDate.AddDays(r.HowManyDays) > reservationDate));

            if (ifDeskHasAnyConflictingReservations)
            {
                return BadRequest("This desk has already been reserved for the selected time period.");
            }

            if (howManyDays > 7)
            {
                return BadRequest("Reservation cannot be longer than 7 days.");
            }

            if (reservationDate.Date < DateTime.Now.Date)
            {
                return BadRequest("Reservation date cannot be in the past.");
            }

            _context.Reservations.Add(new Reservation()
            {
                DeskId = deskId,
                UserId = userId,
                BookingDate = DateTime.Now,
                ReservationDate = reservationDate,
                HowManyDays = howManyDays
            });
            _context.SaveChanges();
            return Ok("Desk reserved successfully.");
        }

        [HttpGet("{locationId}/desks")]
        public IActionResult GetDesks(int locationId, bool? isAvailable = null)
        {
            var location = _context.Locations.Find(locationId);
            if (location == null)
            {
                return NotFound("Location not found.");
            }

            var allDesksInLocation = _context.Desks.Where(d => d.LocationId == locationId);

            if (isAvailable != null)
            {
                allDesksInLocation = allDesksInLocation.Where(d => d.IsAvailable == isAvailable);
            }

            var desks = allDesksInLocation.ToList();

            var desksDto = desks.Select(d => new DeskDetailsDto()
            {
                Id = d.Id,
                IsAvailable = d.IsAvailable,
                LocationName = d.Location.Name,
                Reservations = d.Reservations.Select(r => new ReservationDto()
                {
                    Id = r.Id,
                    BookingDate = r.BookingDate,
                    ReservationDate = r.ReservationDate,
                    HowManyDays = r.HowManyDays,
                    UserId = r.UserId
                }).ToList()
            }).ToList();

            if (!desks.Any())
            {
                return Ok("No desks found in this location.");
            }

            return Ok(desksDto);
        }

        [HttpGet("{locationId}/desks")]
        public IActionResult GetDesksByAvailability(int locationId, DateTime startDate, DateTime endDate, bool desksStatus)
        {
            var location = _context.Locations.Find(locationId);
            if (location == null)
            {
                return NotFound("Location not found.");
            }

            var allDesksInLocation = _context.Desks.Include(d => d.Reservations).Where(d => d.LocationId == locationId).ToList();

            var availableDesks = allDesksInLocation.Where(d => !d.Reservations.Any(r => (r.ReservationDate < endDate && r.ReservationDate.AddDays(r.HowManyDays) > startDate))).ToList();
            if (!desksStatus)
            {
                availableDesks = allDesksInLocation.Except(availableDesks).ToList();
            }
            var desksDto = availableDesks.Select(d => new DeskDetailsDto()
            {
                Id = d.Id,
                IsAvailable = d.IsAvailable,
                LocationName = d.Location.Name,
                Reservations = d.Reservations.Select(r => new ReservationDto()
                {
                    Id = r.Id,
                    BookingDate = r.BookingDate,
                    ReservationDate = r.ReservationDate,
                    HowManyDays = r.HowManyDays,
                    UserId = r.UserId
                }).ToList()
            }).ToList();

            if (!desksDto.Any())
            {
                return Ok("No desks available in this location during the specified time period.");
            }

            return Ok(desksDto);
        }

        [HttpGet("{deskId}/details")]
        public IActionResult GetDeskDetails(int deskId)
        {
            var desk = _context.Desks.Include(d => d.Location)
                                     .Include(d => d.Reservations)
                                     .FirstOrDefault(d => d.Id == deskId);
            if (desk == null)
            {
                return NotFound("Desk not found.");
            }

            var isAdmin = User?.IsInRole("Admin") ?? false;

            var deskDetailsDto = new DeskDetailsDto
            {
                Id = desk.Id,
                IsAvailable = desk.IsAvailable,
                LocationName = desk.Location.Name,
                Reservations = isAdmin ? desk.Reservations.Select(r => new ReservationDto
                {
                    Id = r.Id,
                    BookingDate = r.BookingDate,
                    ReservationDate = r.ReservationDate,
                    HowManyDays = r.HowManyDays,
                    UserId = r.UserId
                }).ToList()
                : new List<ReservationDto>()
            };

            return Ok(deskDetailsDto);
        }

        [HttpPut("{deskId}/changeReservationDate")]
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

        [HttpPut("{deskId}/changeReservationDesk")]
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