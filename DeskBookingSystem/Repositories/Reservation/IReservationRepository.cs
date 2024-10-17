using DeskBookingSystem.Data.Models;

namespace DeskBookingSystem.Repositories
{
    public interface IReservationRepository
    {
        List<Reservation> GetAllByDeskId(int deskId);

        Reservation GetById(int id);

        void Update(Reservation reservation);

        void UpdateReservation(Reservation reservation, int daysCount, DateTime newDate);

        public void Add(Reservation reservation);
    }
}