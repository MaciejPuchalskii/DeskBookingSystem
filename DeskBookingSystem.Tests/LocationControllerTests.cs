namespace DeskBookingSystem.Tests
{
    public class LocationControllerTests
    {
        private BookingContext InMemoryContext()
        {
            var options = new DbContextOptionsBuilder<BookingContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            return new BookingContext(options);
        }

        [Fact]
        public void GetDesks_ReturnsNotFound_WhenLocationDoesNotExist()
        {
            // Arrange
            var context = InMemoryContext();
            var controller = new LocationController(context);

            // Act
            var result = controller.GetDesks(1000) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Location not found.", result.Value);
        }

        [Fact]
        public void GetDesks_ReturnsOk_WhenNoDesksAreFound()
        {
            // Arrange
            var context = InMemoryContext();
            context.Locations.Add(new Location { Id = 1, Name = "Biuro Główne - Kraków" });
            context.SaveChanges();

            var controller = new LocationController(context);

            // Act
            var result = controller.GetDesks(1, null) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("No desks found in this location.", result.Value);
        }

        [Fact]
        public void GetDesks_ReturnsOk_WithAvailableDesks()
        {
            // Arrange
            var context = InMemoryContext();
            context.Locations.Add(new Location { Id = 1, Name = "Biuro Główne - Kraków" });
            context.Desks.Add(new Desk { Id = 1, IsAvailable = true, LocationId = 1 });
            context.Desks.Add(new Desk { Id = 2, IsAvailable = true, LocationId = 1 });
            context.Desks.Add(new Desk { Id = 3, IsAvailable = false, LocationId = 1 });
            context.SaveChanges();

            var controller = new LocationController(context);

            // Act
            var result = controller.GetDesks(1, true) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var desks = result.Value as List<DeskDetailsDto>;
            Assert.NotNull(desks);
            Assert.Equal(2, desks.Count);
        }

        [Fact]
        public void GetDesks_ReturnsOk_WithUnavailableDesks()
        {
            // Arrange
            var context = InMemoryContext();
            context.Locations.Add(new Location { Id = 1, Name = "Biuro Główne - Kraków" });
            context.Desks.Add(new Desk { Id = 1, IsAvailable = true, LocationId = 1 });
            context.Desks.Add(new Desk { Id = 2, IsAvailable = false, LocationId = 1 });
            context.SaveChanges();

            var controller = new LocationController(context);

            // Act
            var result = controller.GetDesks(1, false) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var desks = result.Value as List<DeskDetailsDto>;
            Assert.NotNull(desks);
            Assert.Equal(1, desks.Count);
        }
    }
}