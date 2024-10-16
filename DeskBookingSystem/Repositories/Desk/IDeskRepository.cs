using DeskBookingSystem.Dto;
using DeskBookingSystem.Models;
using DeskBookingSystem.Services;

namespace DeskBookingSystem.Repositories
{
    public interface IDeskRepository
    {
        public void Add(Desk desk);

        public Desk GetById(int id);

        public bool Remove(Desk desk);

        public void ChangeDeskAvailability(Desk desk, bool newStatus);

        public Desk GetDetails(int id);
    }
}