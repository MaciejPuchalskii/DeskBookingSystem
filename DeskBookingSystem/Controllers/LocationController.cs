using DeskBookingSystem.Data;
using DeskBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskBookingSystem.Controllers
{
    public class LocationController : Controller
    {
        private readonly BookingContext _context;
        private readonly IConfiguration _configuration;

        public LocationController(BookingContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addLocation")]
        public IActionResult AddLocation(string locationName)
        {
            if (string.IsNullOrEmpty(locationName))
            {
                return BadRequest("Location name cannot be empty.");
            }

            var existingLocation = _context.Locations.FirstOrDefault(l => l.Name == locationName);
            if (existingLocation != null)
            {
                return BadRequest("Location with this name already exists.");
            }

            var newLocation = new Location
            {
                Name = locationName
            };

            _context.Locations.Add(newLocation);
            _context.SaveChanges();

            return Ok($"Location '{locationName}' added successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{locationId}/remove")]
        public IActionResult RemoveLocation(int locationId)
        {
            var location = _context.Locations.Include(l => l.Desks).FirstOrDefault(l => l.Id == locationId);
            if (location == null)
            {
                return NotFound("Location not found.");
            }

            if (location.Desks.Any())
            {
                return BadRequest("Cannot remove location with assigned desks.");
            }

            _context.Locations.Remove(location);
            _context.SaveChanges();

            return Ok($"Location '{location.Name}' removed successfully.");
        }

        [HttpGet("{locationId}/GetDesks")]
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

        [HttpGet("{locationId}/GetDesks?Status={status}")]
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
    }
}