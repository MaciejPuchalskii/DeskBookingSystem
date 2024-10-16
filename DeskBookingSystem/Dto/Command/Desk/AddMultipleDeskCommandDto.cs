namespace DeskBookingSystem.Dto
{
    public class AddMultipleDeskCommandDto
    {
        public int LocationId { get; set; }
        public int Amount { get; set; }
        public bool IsOperational { get; set; }
    }
}