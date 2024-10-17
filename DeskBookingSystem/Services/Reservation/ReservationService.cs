using DeskBookingSystem.Data.Models;
using DeskBookingSystem.Dto;
using DeskBookingSystem.Repositories;

namespace DeskBookingSystem.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IDeskRepository _deskRepository;

        public ReservationService(IReservationRepository reservationRepository, IDeskRepository deskRepository)
        {
            _reservationRepository = reservationRepository;
            _deskRepository = deskRepository;
        }

        public ChangeReservationDateResponseDto ChangeReservationDate(ChangeReservationDateCommandDto changeReservationDateCommandDto)
        {
            var reservation = _reservationRepository.GetById(changeReservationDateCommandDto.ReservationId);

            if (reservation == null)
            {
                throw new Exception("Reservation not found.");
            }

            if ((reservation.ReservationDate - DateTime.Now).TotalHours <= 24)
            {
                throw new Exception("You can't change the reservation less than 24 hours before the reservation.");
            }

            var reservationsOfThisDesk = _reservationRepository.GetAllByDeskId(reservation.DeskId);

            bool isDeskReserved = reservationsOfThisDesk.Any(r =>
     (changeReservationDateCommandDto.NewDate < r.BookingDate.AddDays(r.DaysCount) &&
      changeReservationDateCommandDto.NewDate.AddDays(reservation.DaysCount) > r.BookingDate));

            if (isDeskReserved)
            {
                throw new Exception("The new desk is not available for the selected time period.");
            }

            if (changeReservationDateCommandDto.DaysCount > 7)
            {
                throw new Exception("Reservation cannot be longer than 7 days.");
            }

            _reservationRepository.UpdateReservation(reservation, changeReservationDateCommandDto.DaysCount, changeReservationDateCommandDto.NewDate);

            return new ChangeReservationDateResponseDto()
            {
                DeskId = reservation.DeskId,
                ReservationId = reservation.Id,
                UserId = reservation.UserId,
                startDate = reservation.ReservationDate,
                endDate = reservation.ReservationDate.AddDays(reservation.DaysCount),
            };
        }

        public ChangeReservationDeskResponseDto ChangeReservationDesk(ChangeReservationDeskCommandDto changeReservationDeskCommandDto)
        {
            var reservation = _reservationRepository.GetById(changeReservationDeskCommandDto.Id);
            if (reservation == null)
            {
                throw new Exception("Reservation not found.");
            }

            if ((reservation.ReservationDate - DateTime.Now).TotalHours <= 24)
            {
                throw new Exception("You can't change the desk less than 24 hours before the reservation.");
            }

            var newDeskReservations = _reservationRepository.GetAllByDeskId(changeReservationDeskCommandDto.DeskId);
            bool hasConflictingReservation = newDeskReservations.Any(r =>
                                                                           r.ReservationDate < reservation.ReservationDate.AddDays(reservation.DaysCount) &&
                                                                           r.ReservationDate.AddDays(r.DaysCount) > reservation.ReservationDate);

            if (hasConflictingReservation)
            {
                throw new Exception("The new desk is not available for the selected time period.");
            }

            reservation.DeskId = changeReservationDeskCommandDto.DeskId;
            _reservationRepository.Update(reservation);

            return new ChangeReservationDeskResponseDto
            {
                ReservationId = reservation.Id,
                DeskId = reservation.DeskId
            };
        }

        public ReserveDeskResponseDto Reserve(ReserveDeskCommandDto reserveDeskCommandDto)
        {
            var desk = _deskRepository.GetById(reserveDeskCommandDto.DeskId);
            if (desk == null)
            {
                throw new Exception("Desk not found.");
            }

            if (!desk.IsOperational)
            {
                throw new Exception("Desk is not available for reservation.");
            }

            if (reserveDeskCommandDto.DaysCount > 7)
            {
                throw new Exception("Reservation cannot be longer than 7 days.");
            }
            if (reserveDeskCommandDto.ReservationDate < DateTime.Now.Date)
            {
                throw new Exception("Reservation date cannot be in the past.");
            }

            var ifDeskHasAnyConflictingReservations = desk.Reservations.Any(r => r.ReservationDate < reserveDeskCommandDto.ReservationDate.AddDays(reserveDeskCommandDto.DaysCount) && r.ReservationDate.AddDays(r.DaysCount) > reserveDeskCommandDto.ReservationDate);

            if (ifDeskHasAnyConflictingReservations)
            {
                throw new Exception("This desk has already been reserved for the selected time period.");
            }

            var reservation = new Reservation()
            {
                DeskId = reserveDeskCommandDto.DeskId,
                UserId = reserveDeskCommandDto.UserId,
                BookingDate = DateTime.Now,
                ReservationDate = reserveDeskCommandDto.ReservationDate,
                DaysCount = reserveDeskCommandDto.DaysCount
            };

            _reservationRepository.Add(reservation);

            return new ReserveDeskResponseDto()
            {
                ReservationId = reservation.Id,
                DeskId = reservation.DeskId,
                EndDate = reservation.ReservationDate.AddDays(reservation.DaysCount),
                StartDate = reservation.ReservationDate
            };
        }
    }
}