using DeskBookingSystem.Dto;
using DeskBookingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPut("{reservationId}/date")]
        public ActionResult<ChangeReservationDateResponseDto> ChangeReservationDate([FromRoute] int reservationId, int userId, DateTime newDate, int howManyDays)
        {
            try
            {
                var queryDto = new ChangeReservationDateCommandDto()
                {
                    ReservationId = reservationId,
                    UserId = userId,
                    NewDate = newDate,
                    HowManyDays = howManyDays
                };
                var response = _reservationService.ChangeReservationDate(queryDto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Reservation not found.")
                {
                    return NotFound(ex.Message);
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

        [HttpPut("{reservationId}/desk")]
        public ActionResult<ChangeReservationDeskResponseDto> ChangeReservationDesk(int reservationId, [FromQuery] int deskId)
        {
            try
            {
                var queryDto = new ChangeReservationDeskCommandDto
                {
                    Id = reservationId,
                    DeskId = deskId
                };
                var response = _reservationService.ChangeReservationDesk(queryDto);
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

        [HttpPost("desk")]
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