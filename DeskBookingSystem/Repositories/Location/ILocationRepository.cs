using DeskBookingSystem.Models;

namespace DeskBookingSystem.Repositories
{
    public interface ILocationRepository
    {
        public bool ExistLocation(int locationId);

        public bool ExistLocation(string name);

        public void Add(Location location);

        public Location GetById(int id);

        public bool Remove(Location location);
    }
}