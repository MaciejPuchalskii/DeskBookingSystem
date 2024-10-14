namespace DeskBookingSystem.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public List<ReservationDto> Reservations { get; set; } = new List<ReservationDto>();
    }
}