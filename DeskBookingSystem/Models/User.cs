namespace DeskBookingSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}