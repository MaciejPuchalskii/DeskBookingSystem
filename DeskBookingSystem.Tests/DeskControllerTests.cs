namespace DeskBookingSystem.Tests
{
    public class DeskControllerTests
    {
        private BookingContext InMemoryContext()
        {
            var options = new DbContextOptionsBuilder<BookingContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            return new BookingContext(options);
        }

        [Fact]
        public void ReserveDesk_ReturnsNotFound_WhenDeskDoesNotExist()
        {
            // Arrange
            var context = InMemoryContext();
            var controller = new DeskController(context);

            // Act
            var result = controller.ReserveDesk(1000, 1, DateTime.Now.AddDays(1), 3) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Desk not found.", result.Value);
        }

        [Fact]
        public void ReserveDesk_ReturnsBadRequest_WhenDeskIsNotAvailable()
        {
            // Arrange
            var context = InMemoryContext();
            context.Desks.Add(new Desk { Id = 1, IsAvailable = false, LocationId = 1 });
            context.SaveChanges();

            var controller = new DeskController(context);

            // Act
            var result = controller.ReserveDesk(1, 1, DateTime.Now.AddDays(1), 3) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Desk is not available for reservation.", result.Value);
        }

        [Fact]
        public void ReserveDesk_ReturnsBadRequest_WhenReservationExceedsDayLimit()
        {
            // Arrange
            var context = InMemoryContext();
            context.Desks.Add(new Desk { Id = 1, IsAvailable = true, LocationId = 1 });
            context.SaveChanges();

            var controller = new DeskController(context);

            // Act
            var result = controller.ReserveDesk(1, 1, DateTime.Now.AddDays(1), 10) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Reservation cannot be longer than 7 days.", result.Value);
        }

        [Fact]
        public void ReserveDesk_ReturnsBadRequest_WhenReservationConflictsWithExisting()
        {
            // Arrange
            var context = InMemoryContext();
            var desk = new Desk { Id = 1, IsAvailable = true, LocationId = 1 };
            var existingReservation = new Reservation
            {
                DeskId = 1,
                UserId = 2,
                ReservationDate = DateTime.Now.AddDays(2),
                HowManyDays = 3
            };
            context.Desks.Add(desk);
            context.Reservations.Add(existingReservation);
            context.SaveChanges();

            var controller = new DeskController(context);

            // Act
            var result = controller.ReserveDesk(1, 1, DateTime.Now.AddDays(2), 3) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("This desk has already been reserved for the selected time period.", result.Value);
        }

        [Fact]
        public void ReserveDesk_ReturnsOk_WhenReservationIsSuccessful()
        {
            // Arrange
            var context = InMemoryContext();
            context.Desks.Add(new Desk { Id = 1, IsAvailable = true, LocationId = 1 });
            context.SaveChanges();

            var controller = new DeskController(context);

            // Act
            var result = controller.ReserveDesk(1, 1, DateTime.Now.AddDays(2), 3) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Desk reserved successfully.", result.Value);
        }

        [Fact]
        public void GetDesks_ReturnsNotFound_WhenLocationDoesNotExist()
        {
            // Arrange
            var context = InMemoryContext();
            var controller = new DeskController(context);

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

            var controller = new DeskController(context);

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

            var controller = new DeskController(context);

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

            var controller = new DeskController(context);

            // Act
            var result = controller.GetDesks(1, false) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var desks = result.Value as List<DeskDetailsDto>;
            Assert.NotNull(desks);
            Assert.Equal(1, desks.Count);
        }

        [Fact]
        public void GetDeskDetails_ReturnsNotFound_WhenDeskDoesNotExist()
        {
            // Arrange
            var context = InMemoryContext();
            var controller = new DeskController(context);

            // Act
            var result = controller.GetDeskDetails(1000) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Desk not found.", result.Value);
        }

        [Fact]
        public void GetDeskDetails_ReturnsOk_WithDeskDetails()
        {
            // Arrange
            var context = InMemoryContext();
            var location = new Location { Id = 1, Name = "Biuro Główne - Kraków" };
            var desk = new Desk { Id = 1, IsAvailable = true, LocationId = 1, Location = location };
            context.Locations.Add(location);
            context.Desks.Add(desk);
            context.SaveChanges();

            var controller = new DeskController(context);

            // Act
            var result = controller.GetDeskDetails(1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var returnedDesk = result.Value as DeskDetailsDto;
            Assert.NotNull(returnedDesk);
            Assert.Equal(1, returnedDesk.Id);
            Assert.Equal("Biuro Główne - Kraków", returnedDesk.LocationName);
        }
    }
}