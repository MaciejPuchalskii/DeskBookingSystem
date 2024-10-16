using DeskBookingSystem.Dto;
using DeskBookingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeskController : Controller
    {
        private readonly IDeskService _deskService;
        private readonly ILocationService _locationService;

        public DeskController(IDeskService deskService, ILocationService locationService)
        {
            _deskService = deskService;
            _locationService = locationService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/desks")]
        public ActionResult<AddDeskResponseDto> AddDesk(AddDeskCommandDto addDeskCommandDto)
        {
            try
            {
                _locationService.ExistLocation(addDeskCommandDto.LocationId);

                var desk = _deskService.Add(addDeskCommandDto);
                return Ok(desk);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Location not found.")
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("multiple")]
        public ActionResult<AddMultipleDeskResponseDto> AddDesks(AddMultipleDeskCommandDto addMutlipleDeskCommandDto)
        {
            try
            {
                _locationService.ExistLocation(addMutlipleDeskCommandDto.LocationId);

                var responseDtos = new List<AddDeskResponseDto>();
                for (int i = 0; i < addMutlipleDeskCommandDto.Amount; i++)
                {
                    responseDtos.Add(_deskService.Add(new AddDeskCommandDto
                    {
                        LocationId = addMutlipleDeskCommandDto.LocationId,
                        IsAvailable = addMutlipleDeskCommandDto.IsAvailable
                    }));
                }

                var response = new AddMultipleDeskResponseDto()
                {
                    AddedDesks = responseDtos,
                    TotalAdded = responseDtos.Count
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Location not found.")
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("/{deskId}")]
        public ActionResult<RemoveDeskResponseDto> RemoveDesk( int deskId)
        {
            try
            {
                var response = _deskService.Remove(new RemoveDeskCommandDto() { Id = deskId });
                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Desk not found.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "Cannot remove desk with existing reservations.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "Failed to remove desk.")
                {
                    return StatusCode(500, ex.Message);
                }
                else
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("")]
        public ActionResult<ChangeDeskAvailabiltyResponseDto> ChangeDeskAvailability(ChangeDeskAvailabiltyCommandDto setDeskCommandDto)
        {
            try
            {
                var desk = _deskService.ChangeDeskAvailability(setDeskCommandDto);
                return Ok(desk);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Desk not found.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "Cannot disable a desk with existing reservations.")
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }

        [HttpGet("/{deskId}/details")]
        public ActionResult<GetDeskDetailsResponseDto> GetDeskDetails(int deskId)
        {
            try
            {
                var deskDetailsDto = new GetDeskDetailsQueryDto
                {
                    Id = deskId
                };
                var details = _deskService.GetDeskDetails(deskDetailsDto);
                return Ok(details);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Desk not found.")
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