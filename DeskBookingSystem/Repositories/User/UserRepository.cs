using DeskBookingSystem.Data;
using DeskBookingSystem.Data.Models;

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

        public bool DoesUsernamExist(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public User GetUser(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}