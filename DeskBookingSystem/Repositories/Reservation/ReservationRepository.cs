using DeskBookingSystem.Data;
using DeskBookingSystem.Data.Models;

namespace DeskBookingSystem.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly BookingContext _context;

        public ReservationRepository(BookingContext context)
        {
            _context = context;
        }

        public void Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
        }

        public List<Reservation> GetAllByDeskId(int deskId)
        {
            return _context.Reservations.Where(r => r.DeskId == deskId).ToList();
        }

        public Reservation GetById(int id)
        {
            return _context.Reservations.Find(id);
        }

        public void Update(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            _context.SaveChanges();
        }

        public void UpdateReservation(Reservation reservation, int daysCount, DateTime newDate)
        {
            reservation.ReservationDate = newDate;
            reservation.DaysCount = daysCount;
            _context.SaveChanges();
        }
    }
}