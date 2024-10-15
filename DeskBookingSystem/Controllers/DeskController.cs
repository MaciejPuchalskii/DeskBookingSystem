using DeskBookingSystem.Data;
using DeskBookingSystem.Dto;
using DeskBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public IActionResult AddDesk(int locationId, bool status)
        {
            var location = _context.Locations.Find(locationId);
            if (location == null)
            {
                return BadRequest("Location not found.");
            }

            _context.Desks.Add(new Desk() { IsAvailable = status, LocationId = locationId });
            _context.SaveChanges();

            return Ok($"Desks added successfully in {location.Name}.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addMultiple")]
        public IActionResult AddDesks(int locationId, int amount, bool status)
        {
            var location = _context.Locations.Find(locationId);
            if (location == null)
            {
                return BadRequest("Location not found.");
            }

            var desks = new List<Desk>();
            for (int i = 0; i < amount; i++)
            {
                desks.Add(new Desk
                {
                    IsAvailable = status,
                    LocationId = locationId
                });
            }

            _context.Desks.AddRange(desks);
            _context.SaveChanges();

            return Ok($"{amount} desks added successfully in {location.Name}.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{deskId}/remove")]
        public IActionResult RemoveDesk(int deskId)
        {
            var desk = _context.Desks.Include(d => d.Reservations).FirstOrDefault(d => d.Id == deskId);

            if (desk == null)
            {
                return NotFound("Desk not found.");
            }
            if (desk.Reservations.Any())
            {
                return BadRequest("Cannot remove desk with existing reservations.");
            }
            _context.Desks.Remove(desk);
            _context.SaveChanges();

            return Ok("Desk removed successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{deskId}/disable")]
        public IActionResult SetDeskUnavailable(int deskId)
        {
            var desk = _context.Desks.Include(d => d.Reservations).FirstOrDefault(d => d.Id == deskId);
            if (desk == null)
            {
                return NotFound("Desk not found.");
            }

            if (desk.Reservations.Any())
            {
                return BadRequest("Cannot disable a desk with existing reservations.");
            }

            if (!desk.IsAvailable)
            {
                return BadRequest("This desk is already disabled.");
            }

            desk.IsAvailable = false;
            _context.SaveChanges();

            return Ok("Desk disabled successfully.");
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
    }
}