using DeskBookingSystem.Dto;
using DeskBookingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskBookingSystem.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/locations")]
        public ActionResult<AddLocationResponseDto> AddLocation(AddLocationCommandDto addLocationCommandDto)
        {
            try
            {
                var location = _locationService.Add(addLocationCommandDto);
                return Ok(location);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Location name cannot be empty.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "Location with this name already exists.")
                {
                    return Conflict(ex.Message);
                }
                else
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("/locations/{locationId}/remove")]
        public ActionResult<RemoveLocationResponseDto> RemoveLocation(int locationId)
        {
            try
            {
                var response = _locationService.Remove(new RemoveLocationCommandDto() { Id = locationId });
                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Location not found.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "Cannot remove location with assigned desks.")
                {
                    return Conflict(ex.Message);
                }
                else if (ex.Message == "Failed to remove location.")
                {
                    return Conflict(ex.Message);
                }
                else
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }

        [HttpGet("/locations/{locationId}/desks")]
        public ActionResult<GetDesksFromLocationResponseDto> GetDesks(int locationId, bool? areAvailable = null)
        {
            try
            {
                var queryDto = new GetDesksFromLocationQueryDto
                {
                    LocationId = locationId,
                    areAvailable = areAvailable
                };
                var desks = _locationService.GetDesks(queryDto);
                return Ok(desks);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Location not found.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "No desks found in this location.")
                {
                    return NotFound(ex.Message);
                }
                else
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }

        [HttpGet("/locations/{locationId}/desks/status")]
        public ActionResult<GetDesksByAvailabilityResponseDto> GetDesksByAvailability(int locationId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var queryDto = new GetDesksByAvailabilityQueryDto
                {
                    LocationId = locationId,
                    StartDate = startDate,
                    EndDate = endDate,
                };
                var response = _locationService.GetDesksByAvailability(queryDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Location not found.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "No desks found in this location.")
                {
                    return NotFound(ex.Message);
                }
                else if (ex.Message == "No desks available in this location during the specified time period.")
                {
                    return NotFound(ex.Message);
                }
                else
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }
    }
}