using DeskBookingSystem.Dto;

namespace DeskBookingSystem.Services
{
    public interface IUserService
    {
        RegisterResponseDto Register(RegisterCommandDto registerCommandDto);

        LoginResponseDto Login(LoginCommandDto loginCommandDto);
    }
}