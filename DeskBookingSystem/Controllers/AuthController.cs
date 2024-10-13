using DeskBookingSystem.Data;
using DeskBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DeskBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BookingContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(BookingContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public ActionResult<User> Register(RegisterDto requestUser)
        {
            if (_context.Users.Any(u => u.UserName == requestUser.UserName))
            {
                return BadRequest("User with this username already exists.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(requestUser.Password);

            var newUser = new User()
            {
                Name = requestUser.Name,
                Surname = requestUser.Surname,
                UserName = requestUser.UserName,
                Email = requestUser.Email,
                PasswordHash = passwordHash,
                IsAdmin = requestUser.IsAdmin,
                Reservations = new List<Reservation>()
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();

            var userDto = new UserDto()
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Surname = newUser.Surname,
                UserName = newUser.UserName,
                Email = newUser.Email,
                Reservations = new List<ReservationDto>()
            };

            return Ok(userDto);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(LoginDto loginUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == loginUser.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
            {
                return BadRequest("Invalid username or password.");
            }

            string token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}