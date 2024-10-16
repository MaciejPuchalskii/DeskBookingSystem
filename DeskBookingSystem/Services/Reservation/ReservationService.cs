using DeskBookingSystem.Dto;
using DeskBookingSystem.Models;
using DeskBookingSystem.Repositories;
using Microsoft.EntityFrameworkCore;

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

            var reservationsOfThisDesk = _reservationRepository.GetAllReservationsByDeskId(reservation.DeskId);

            bool isDeskReserved = reservationsOfThisDesk.Any(r =>
     (changeReservationDateCommandDto.NewDate < r.BookingDate.AddDays(r.HowManyDays) &&
      changeReservationDateCommandDto.NewDate.AddDays(reservation.HowManyDays) > r.BookingDate));

            if (isDeskReserved)
            {
                throw new Exception("The new desk is not available for the selected time period.");
            }

            if (changeReservationDateCommandDto.HowManyDays > 7)
            {
                throw new Exception("Reservation cannot be longer than 7 days.");
            }

            _reservationRepository.UpdateReservation(reservation, changeReservationDateCommandDto.HowManyDays, changeReservationDateCommandDto.NewDate);

            return new ChangeReservationDateResponseDto()
            {
                DeskId = reservation.DeskId,
                ReservationId = reservation.Id,
                UserId = reservation.UserId,
                startDate = reservation.ReservationDate,
                endDate = reservation.ReservationDate.AddDays(reservation.HowManyDays),
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

            var newDeskReservations = _reservationRepository.GetAllReservationsByDeskId(changeReservationDeskCommandDto.DeskId);
            bool hasConflictingReservation = newDeskReservations.Any(r =>
                                                                           r.ReservationDate < reservation.ReservationDate.AddDays(reservation.HowManyDays) &&
                                                                           r.ReservationDate.AddDays(r.HowManyDays) > reservation.ReservationDate);

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

            if (reserveDeskCommandDto.HowManyDays > 7)
            {
                throw new Exception("Reservation cannot be longer than 7 days.");
            }
            if (reserveDeskCommandDto.ReservationDate < DateTime.Now.Date)
            {
                throw new Exception("Reservation date cannot be in the past.");
            }

            var ifDeskHasAnyConflictingReservations = desk.Reservations.Any(r => r.ReservationDate < reserveDeskCommandDto.ReservationDate.AddDays(reserveDeskCommandDto.HowManyDays) && r.ReservationDate.AddDays(r.HowManyDays) > reserveDeskCommandDto.ReservationDate);

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
                HowManyDays = reserveDeskCommandDto.HowManyDays
            };

            _reservationRepository.Add(reservation);

            return new ReserveDeskResponseDto()
            {
                ReservationId = reservation.Id,
                DeskId = reservation.DeskId,
                EndDate = reservation.ReservationDate.AddDays(reservation.HowManyDays),
                StartDate = reservation.ReservationDate
            };
        }
    }
}