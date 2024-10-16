using DeskBookingSystem.Data;
using DeskBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace DeskBookingSystem.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly BookingContext _context;

        public LocationRepository(BookingContext context)
        {
            _context = context;
        }

        public void Add(Location location)
        {
            _context.Locations.Add(location);
            _context.SaveChanges();
        }

        public bool ExistLocation(int locationId)
        {
            return _context.Locations.Any(l => l.Id == locationId);
        }

        public bool ExistLocation(string name)
        {
            return _context.Locations.Any(l => l.Name == name);
        }

        public Location GetById(int id)
        {
            return _context.Locations.Include(l => l.Desks).ThenInclude(d=>d.Reservations).FirstOrDefault(l => l.Id == id);
        }

        public bool Remove(Location location)
        {
            _context.Locations.Remove(location);
            int changes = _context.SaveChanges();
            return changes > 0;
        }
    }
}