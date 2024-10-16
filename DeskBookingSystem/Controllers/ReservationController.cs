using DeskBookingSystem.Dto;
using DeskBookingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskBookingSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPut("/reservations/{reservationId}/changeReservationDate")]
        public ActionResult<ChangeReservationDateResponseDto> ChangeReservationDate(ChangeReservationDateCommandDto changeReservationDateCommandDto)
        {
            try
            {
                var response = _reservationService.ChangeReservationDate(changeReservationDateCommandDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Reservation not found.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "You can't change the reservation less than 24 hours before the reservation.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "The new desk is not available for the selected time period.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "Reservation cannot be longer than 7 days.")
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }

        [HttpPut("{reservationId}/changeDesk")]
        public ActionResult<ChangeReservationDeskResponseDto> ChangeReservationDesk(ChangeReservationDeskCommandDto changeReservationDeskCommandDto)
        {
            try
            {
                var response = _reservationService.ChangeReservationDesk(changeReservationDeskCommandDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Reservation not found.")
                {
                    return NotFound(ex.Message);
                }
                else if (ex.Message == "You can't change the desk less than 24 hours before the reservation.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "The new desk is not available for the selected time period.")
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }

        [HttpPost("/reserveDesk{deskId}")]
        public ActionResult<ReserveDeskResponseDto> ReserveDesk(ReserveDeskCommandDto reserveDeskCommandDto)
        {
            try
            {
                var response = _reservationService.Reserve(reserveDeskCommandDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Desk not found.")
                {
                    return NotFound(ex.Message);
                }
                else if (ex.Message == "Desk is not available for reservation.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "Reservation cannot be longer than 7 days.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "Reservation date cannot be in the past.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "This desk has already been reserved for the selected time period.")
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }
    }
}