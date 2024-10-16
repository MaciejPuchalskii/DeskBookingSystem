using DeskBookingSystem.Repositories;
using DeskBookingSystem.Services;

namespace DeskBookingSystem.Configurations
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IReservationService, ReservationService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDeskRepository, DeskRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
        }
    }
}