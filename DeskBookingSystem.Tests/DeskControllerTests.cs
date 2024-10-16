using DeskBookingSystem.Dto;

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

        [Fact]
        public void AddDesk_ReturnsOk_WhenDeskIsAdded()
        {
            // Arrange
            var context = InMemoryContext();
            context.Locations.Add(new Location() { Id = 6, Name = "Biuro - Opole" });
            context.SaveChanges();
            var controller = new DeskController(context);

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
            var controller = new DeskController(context);

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

            var controller = new DeskController(context);

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
            var controller = new DeskController(context);

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

            var controller = new DeskController(context);

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
            var controller = new DeskController(context);

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

            var controller = new DeskController(context);

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
            var controller = new DeskController(context);

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
            var controller = new DeskController(context);

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

            var controller = new DeskController(context);

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

            var controller = new DeskController(context);

            // Act
            var result = controller.SetDeskUnavailable(1) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Cannot disable a desk with existing reservations.", result.Value);
        }
    }
}