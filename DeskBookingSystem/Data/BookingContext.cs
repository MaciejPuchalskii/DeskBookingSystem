using DeskBookingSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DeskBookingSystem.Data
{
    public class BookingContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }

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
              new Desk { Id = 1, IsOperational = true, LocationId = 1 },
              new Desk { Id = 2, IsOperational = false, LocationId = 1 },
              new Desk { Id = 3, IsOperational = true, LocationId = 1 },
              new Desk { Id = 4, IsOperational = true, LocationId = 2 },
              new Desk { Id = 5, IsOperational = true, LocationId = 3 },
              new Desk { Id = 6, IsOperational = false, LocationId = 3 },
              new Desk { Id = 7, IsOperational = false, LocationId = 4 },
              new Desk { Id = 8, IsOperational = false, LocationId = 4 },
              new Desk { Id = 9, IsOperational = true, LocationId = 5 }
            );
        }
    }
}