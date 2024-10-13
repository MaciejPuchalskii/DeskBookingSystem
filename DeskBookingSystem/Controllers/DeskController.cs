using DeskBookingSystem.Data;
using DeskBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            return Ok();
        }

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

            return Ok($"{amount} desks added successfully.");
        }

        [HttpPost("{deskId}/remove")]
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

        [HttpPost("{deskId}/reserve")]
        public IActionResult ReserveDesk(int deskId, int userId, DateTime bookDate, DateTime reservationDate, int howManyDays)
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

            if (howManyDays > 7)
            {
                return BadRequest("Reservation cannot be longer than 7 days.");
            }

            if (reservationDate.Date < DateTime.Now.Date)
            {
                return BadRequest("Reservation date cannot be in the past.");
            }

            desk.IsAvailable = false;

            _context.Reservations.Add(new Reservation() { DeskId = deskId, UserId = userId, BookingDate = bookDate, ReservationDate = reservationDate, HowManyDays = howManyDays });
            _context.SaveChanges();
            return Ok("Desk reserved successfully.");
        }

        [HttpGet("{locationId}/available")]
        public IActionResult GetAvailableDesks(int locationId)
        {
            var location = _context.Locations.Find(locationId);
            if (location == null)
            {
                return NotFound("Location not found.");
            }

            var availableDesks = _context.Desks.Where(d => d.LocationId == locationId && d.IsAvailable).ToList();
            var availableDesksDto = availableDesks.Select(d => new DeskDetailsDto()
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

            if (!availableDesks.Any())
            {
                return Ok("No available desks in this location.");
            }

            return Ok(availableDesksDto);
        }

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

            var deskDetailsDto = new DeskDetailsDto
            {
                Id = desk.Id,
                IsAvailable = desk.IsAvailable,
                LocationName = desk.Location.Name,
                Reservations = desk.Reservations.Select(r => new ReservationDto
                {
                    Id = r.Id,
                    BookingDate = r.BookingDate,
                    ReservationDate = r.ReservationDate,
                    HowManyDays = r.HowManyDays,
                    UserId = r.UserId
                }).ToList()
            };

            return Ok(deskDetailsDto);
        }
    }
}