namespace DeskBookingSystem.Dto
{
    public class GetDesksFromLocationQueryDto
    {
        public int LocationId { get; set; }

        public bool? areAvailable { get; set; }
    }
}