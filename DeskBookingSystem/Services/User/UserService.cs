using DeskBookingSystem.Dto;
using DeskBookingSystem.Models;
using DeskBookingSystem.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DeskBookingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public RegisterResponseDto Register(RegisterCommandDto registerCommandDto)
        {
            if (string.IsNullOrEmpty(registerCommandDto.Password) || string.IsNullOrEmpty(registerCommandDto.UserName))
            {
                throw new Exception("Username or password cannot be empty.");
            }
            if (_userRepository.ExistsByUsername(registerCommandDto.UserName))
            {
                throw new Exception("User with this username already exists.");
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerCommandDto.Password);

            var newUser = new User
            {
                UserName = registerCommandDto.UserName,
                PasswordHash = passwordHash,
                Name = registerCommandDto.Name,
                Surname = registerCommandDto.Surname,
                Email = registerCommandDto.Email,
                IsAdmin = registerCommandDto.IsAdmin
            };

            _userRepository.Add(newUser);

            return new RegisterResponseDto
            {
                Id = newUser.Id,
                Email = newUser.Email,
                UserName = newUser.UserName,
                Name = newUser.Name,
                Surname = newUser.Surname
            };
        }

        public LoginResponseDto Login(LoginCommandDto loginCommandDto)
        {
            if (string.IsNullOrEmpty(loginCommandDto.Password) || string.IsNullOrEmpty(loginCommandDto.UserName))
            {
                throw new Exception("Username or password cannot be empty.");
            }

            if (!_userRepository.ExistsByUsername(loginCommandDto.UserName))
            {
                throw new Exception("User with this username doesn't exist.");
            }

            var user = _userRepository.GetUserIfPasswordCorrect(loginCommandDto.UserName, loginCommandDto.Password);
            if (user == null)
            {
                throw new Exception("Invalid username or password.");
            }

            string token = CreateToken(user);

            return new LoginResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = token
            };
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