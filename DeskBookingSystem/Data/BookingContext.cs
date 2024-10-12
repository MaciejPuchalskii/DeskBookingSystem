using DeskBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DeskBookingSystem.Data
{
    public class BookingContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=bookingDeskSystem.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Location>().HasData(
              new Location { Id = 1, Name = "Biuro Główne - Kraków" },
              new Location { Id = 2, Name = "Biuro - Wrocław" },
              new Location { Id = 3, Name = "Biuro - Warszawa" },
              new Location { Id = 4, Name = "Biuro - Rzeszów" },
              new Location { Id = 5, Name = "Biuro - Bielsko-Biała" }
            );

            modelBuilder.Entity<Desk>().HasData(
              new Desk { Id = 1, IsAvailable = true, LocationId = 1 },
              new Desk { Id = 2, IsAvailable = false, LocationId = 1 },
              new Desk { Id = 3, IsAvailable = true, LocationId = 1 },
              new Desk { Id = 4, IsAvailable = true, LocationId = 2 },
              new Desk { Id = 5, IsAvailable = true, LocationId = 3 },
              new Desk { Id = 6, IsAvailable = false, LocationId = 3 },
              new Desk { Id = 7, IsAvailable = false, LocationId = 4 },
              new Desk { Id = 8, IsAvailable = false, LocationId = 4 },
              new Desk { Id = 9, IsAvailable = true, LocationId = 5 }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Jan", Surname = "Kowalski", Email = "jan.kowalski@mail.com", IsAdmin = true },
                new Employee { Id = 2, Name = "Anna", Surname = "Kowalska", Email = "anna.kowalska@example.com", IsAdmin = false }
            );

            modelBuilder.Entity<Reservation>().HasData(
                new Reservation
                {
                    Id = 1,
                    BookingDate = DateTime.Now,
                    ReservationDate = DateTime.Now.AddDays(1),
                    HowManyDays = 2,
                    DeskId = 1,
                    EmployeeId = 2
                }
            );
        }
    }
}