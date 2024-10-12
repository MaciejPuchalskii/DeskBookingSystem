namespace DeskBookingSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public bool IsAdmin { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}