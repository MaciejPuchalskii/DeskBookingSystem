using DeskBookingSystem.Dto;
using DeskBookingSystem.Models;

namespace DeskBookingSystem.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);

        bool ExistsByUsername(string userName);

        User GetUserIfPasswordCorrect(string userName, string password);
    }
}