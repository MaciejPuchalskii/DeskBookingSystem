namespace DeskBookingSystem.Dto
{
    public class GetDesksByAvailabilityQueryDto
    {
        public int LocationId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}