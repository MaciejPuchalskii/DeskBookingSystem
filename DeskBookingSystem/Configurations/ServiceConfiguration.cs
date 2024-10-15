using DeskBookingSystem.Repositories;
using DeskBookingSystem.Services;

namespace DeskBookingSystem.Configurations
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}