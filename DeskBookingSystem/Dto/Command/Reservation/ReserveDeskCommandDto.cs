namespace DeskBookingSystem.Dto
{
    public class ReserveDeskCommandDto
    {
        public int DeskId { get; set; }

        public int UserId { get; set; }

        public DateTime ReservationDate { get; set; }

        public int HowManyDays { get; set; }
    }
}