namespace DeskBookingSystem.Dto
{
    public class AddDeskCommandDto
    {
        public int LocationId { get; set; }
        public bool IsAvailable { get; set; }
    }
}