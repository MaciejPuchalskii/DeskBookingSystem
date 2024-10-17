using DeskBookingSystem.Data.Models;

namespace DeskBookingSystem.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);

        bool DoesUsernamExist(string userName);

        User GetUser(string userName);
    }
}