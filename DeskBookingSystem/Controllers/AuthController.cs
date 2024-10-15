using DeskBookingSystem.Data;
using DeskBookingSystem.Dto;
using DeskBookingSystem.Models;
using DeskBookingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BookingContext _context;
        private readonly IUserService _userService;

        public AuthController(BookingContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost("register")]
        public ActionResult<User> Register(RegisterCommandDto registerCommandDto)
        {
            try
            {
                var user = _userService.Register(registerCommandDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                if (ex.Message == "User with this username already exists.")
                {
                    return Conflict(ex.Message);
                }
                else if (ex.Message == "Username or password cannot be empty.")
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }

        [HttpPost("login")]
        public ActionResult<User> Login(LoginCommandDto loginUser)
        {
            try
            {
                var user = _userService.Login(loginUser);
                return Ok(user);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Username or password cannot be empty.")
                {
                    return Conflict(ex.Message);
                }
                else if (ex.Message == "User with this username doesn't exist.")
                {
                    return BadRequest(ex.Message);
                }
                else if (ex.Message == "Invalid username or password.")
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