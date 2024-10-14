using DeskBookingSystem.Data;
using DeskBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeskBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly BookingContext _context;

        public AdminController(BookingContext context)
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
        [HttpDelete("removeLocation/{locationId}")]
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
    }
}