using DeskBookingSystem.Data.Models;

namespace DeskBookingSystem.Repositories
{
    public interface ILocationRepository
    {
        public bool DoesLocationExist(int locationId);

        public bool DoesLocationExist(string name);

        public void Add(Location location);

        public Location GetById(int id);

        public bool Remove(Location location);
    }
}