using DeskBookingSystem.Dto;
using DeskBookingSystem.Models;

namespace DeskBookingSystem.Services
{
    public interface IReservationService
    {
        ChangeReservationDateResponseDto ChangeReservationDate(ChangeReservationDateCommandDto changeReservationDateCommandDto);

        ChangeReservationDeskResponseDto ChangeReservationDesk(ChangeReservationDeskCommandDto changeReservationDeskCommandDto);

        ReserveDeskResponseDto Reserve(ReserveDeskCommandDto reserveDeskCommandDto);
    }
}