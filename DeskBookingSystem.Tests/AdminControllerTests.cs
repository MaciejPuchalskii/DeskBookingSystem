namespace DeskBookingSystem.Tests
{
    public class AdminControllerTests
    {
        private BookingContext InMemoryContext()
        {
            var options = new DbContextOptionsBuilder<BookingContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            return new BookingContext(options);
        }

        [Fact]
        public void AddDesk_ReturnsOk_WhenDeskIsAdded()
        {
            // Arrange
            var context = InMemoryContext();
            context.Locations.Add(new Location() { Id = 6, Name = "Biuro - Opole" });
            context.SaveChanges();
            var controller = new AdminController(context);

            // Act
            var result = controller.AddDesk(6, true) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void AddDesk_ReturnsBadRequest_WhenLocationDoesNotExist()
        {
            // Arrange
            var context = InMemoryContext();
            var controller = new AdminController(context);

            // Act
            var result = controller.AddDesk(1000, true) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Location not found.", result.Value);
        }

        [Fact]
        public void AddDesks_ReturnsOkWhenMultipleDesksAreAdded()
        {
            // Arrange
            var context = InMemoryContext();
            context.Locations.Add(new Location() { Id = 6, Name = "Biuro - Opole" });
            context.SaveChanges();

            var controller = new AdminController(context);

            // Act
            var result = controller.AddDesks(6, 5, true) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("5 desks added successfully in Biuro - Opole.", result.Value);
            Assert.Equal(5, context.Desks.Count());
        }

        [Fact]
        public void AddDesks_ReturnsBadRequest_WhenLocationDoesNotExist()
        {
            // Arrange
            var context = InMemoryContext();
            var controller = new AdminController(context);

            // Act
            var result = controller.AddDesks(5, 999, true) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Location not found.", result.Value);
        }

        [Fact]
        public void RemoveDesk_ReturnsOk_WhenDeskIsRemoved()
        {
            // Arrange
            var context = InMemoryContext();
            context.Locations.Add(new Location { Id = 1, Name = "Kraków" });
            context.Desks.Add(new Desk { Id = 1, IsAvailable = true, LocationId = 1 });
            context.SaveChanges();

            var controller = new AdminController(context);

            // Act
            var result = controller.RemoveDesk(1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Desk removed successfully.", result.Value);
        }

        [Fact]
        public void RemoveDesk_ReturnsNotFound_WhenDeskDoesNotExist()
        {
            // Arrange
            var context = InMemoryContext();
            var controller = new AdminController(context);

            // Act
            var result = controller.RemoveDesk(1000) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Desk not found.", result.Value);
        }

        [Fact]
        public void RemoveDesk_ReturnsBadRequest_WhenDeskHasReservations()
        {
            // Arrange
            var context = InMemoryContext();
            var desk = new Desk { Id = 1, IsAvailable = true, LocationId = 1 };
            var reservation = new Reservation { Id = 1, DeskId = 1, ReservationDate = DateTime.Now.AddDays(1), HowManyDays = 2, UserId = 2 };
            context.Desks.Add(desk);
            context.Reservations.Add(reservation);
            context.SaveChanges();

            var controller = new AdminController(context);

            // Act
            var result = controller.RemoveDesk(1) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Cannot remove desk with existing reservations.", result.Value);
        }

        [Fact]
        public void SetDeskUnavailable_ReturnsNotFound_WhenDeskDoesNotExist()
        {
            // Arrange
            var context = InMemoryContext();
            var controller = new AdminController(context);

            // Act
            var result = controller.SetDeskUnavailable(1000) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Desk not found.", result.Value);
        }

        [Fact]
        public void SetDeskUnavailable_ReturnsBadRequest_WhenDeskIsAlreadyDisabled()
        {
            // Arrange
            var context = InMemoryContext();
            context.Desks.Add(new Desk { Id = 1, IsAvailable = false, LocationId = 1 });
            context.SaveChanges();
            var controller = new AdminController(context);

            // Act
            var result = controller.SetDeskUnavailable(1) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("This desk is already disabled.", result.Value);
        }

        [Fact]
        public void SetDeskUnavailable_ReturnsOk_WhenDeskIsDisabledSuccessfully()
        {
            // Arrange
            var context = InMemoryContext();
            context.Desks.Add(new Desk { Id = 1, IsAvailable = true, LocationId = 1 });
            context.SaveChanges();

            var controller = new AdminController(context);

            // Act
            var result = controller.SetDeskUnavailable(1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Desk disabled successfully.", result.Value);

            var desk = context.Desks.Find(1);
            Assert.False(desk!.IsAvailable);
        }

        [Fact]
        public void SetDeskUnavailable_ReturnsBadRequest_WhenDeskHasReservations()
        {
            // Arrange
            var context = InMemoryContext();
            var desk = new Desk { Id = 1, IsAvailable = true, LocationId = 1 };
            context.Desks.Add(desk);
            context.Reservations.Add(new Reservation { DeskId = 1, ReservationDate = DateTime.Now.AddDays(2), HowManyDays = 2, UserId = 1 });
            context.SaveChanges();

            var controller = new AdminController(context);

            // Act
            var result = controller.SetDeskUnavailable(1) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Cannot disable a desk with existing reservations.", result.Value);
        }
    }
}