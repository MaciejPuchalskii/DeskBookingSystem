using DeskBookingSystem.Data;
using DeskBookingSystem.Dto;
using DeskBookingSystem.Models;

namespace DeskBookingSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookingContext _context;

        public UserRepository(BookingContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool ExistsByUsername(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public User GetUserIfPasswordCorrect(string userName, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                return null;
            }

            if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }
    }
}