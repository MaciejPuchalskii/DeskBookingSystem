namespace DeskBookingSystem.Tests
{
    public class ReservationControllerTests
    {
        private BookingContext InMemoryContext()
        {
            var options = new DbContextOptionsBuilder<BookingContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            return new BookingContext(options);
        }
    }
}