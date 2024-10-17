using DeskBookingSystem.Data.Models;

public class ReservationServiceTests
{
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<IDeskRepository> _deskRepositoryMock;
    private readonly ReservationService _service;

    public ReservationServiceTests()
    {
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _deskRepositoryMock = new Mock<IDeskRepository>();
        _service = new ReservationService(_reservationRepositoryMock.Object, _deskRepositoryMock.Object);
    }

    [Fact]
    public void ChangeReservationDate_ShouldUpdateReservation_WhenValid()
    {
        // Arrange
        var reservation = new Reservation
        {
            Id = 1,
            DeskId = 1,
            UserId = 1,
            ReservationDate = DateTime.Now.AddDays(3),
            DaysCount = 2
        };

        _reservationRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(reservation);
        _reservationRepositoryMock.Setup(repo => repo.GetAllByDeskId(It.IsAny<int>())).Returns(new List<Reservation>());

        var commandDto = new ChangeReservationDateCommandDto
        {
            ReservationId = 1,
            UserId = 1,
            NewDate = DateTime.Now.AddDays(4),
            DaysCount = 3
        };

        // Act
        var result = _service.ChangeReservationDate(commandDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.ReservationId);
        _reservationRepositoryMock.Verify(repo => repo.UpdateReservation(reservation, 3, commandDto.NewDate), Times.Once);
    }

    [Fact]
    public void ChangeReservationDate_ShouldThrowException_WhenReservationNotFound()
    {
        // Arrange
        _reservationRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Reservation)null);

        var commandDto = new ChangeReservationDateCommandDto
        {
            ReservationId = 1,
            UserId = 1,
            NewDate = DateTime.Now.AddDays(4),
            DaysCount = 3
        };

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => _service.ChangeReservationDate(commandDto));
        Assert.Equal("Reservation not found.", exception.Message);
    }

    [Fact]
    public void ChangeReservationDesk_ShouldUpdateDesk_WhenValid()
    {
        // Arrange
        var reservation = new Reservation
        {
            Id = 1,
            DeskId = 1,
            UserId = 1,
            ReservationDate = DateTime.Now.AddDays(2),
            DaysCount = 2
        };

        _reservationRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(reservation);
        _reservationRepositoryMock.Setup(repo => repo.GetAllByDeskId(It.IsAny<int>())).Returns(new List<Reservation>());

        var commandDto = new ChangeReservationDeskCommandDto
        {
            Id = 1,
            DeskId = 2
        };

        // Act
        var result = _service.ChangeReservationDesk(commandDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.DeskId);
        _reservationRepositoryMock.Verify(repo => repo.Update(reservation), Times.Once);
    }

    [Fact]
    public void ChangeReservationDesk_ShouldThrowException_WhenDeskIsOccupied()
    {
        // Arrange
        var reservation = new Reservation
        {
            Id = 1,
            DeskId = 1,
            UserId = 1,
            ReservationDate = DateTime.Now.AddDays(2),
            DaysCount = 2
        };

        var conflictingReservation = new Reservation
        {
            Id = 2,
            DeskId = 2,
            ReservationDate = DateTime.Now.AddDays(3),
            DaysCount = 1
        };

        _reservationRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(reservation);
        _reservationRepositoryMock.Setup(repo => repo.GetAllByDeskId(It.IsAny<int>())).Returns(new List<Reservation> { conflictingReservation });

        var commandDto = new ChangeReservationDeskCommandDto
        {
            Id = 1,
            DeskId = 2
        };

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => _service.ChangeReservationDesk(commandDto));
        Assert.Equal("The new desk is not available for the selected time period.", exception.Message);
    }

    [Fact]
    public void Reserve_ShouldCreateReservation_WhenValid()
    {
        // Arrange
        var desk = new Desk
        {
            Id = 1,
            IsOperational = true,
            Reservations = new List<Reservation>()
        };

        _deskRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(desk);

        var reserveCommandDto = new ReserveDeskCommandDto
        {
            DeskId = 1,
            UserId = 1,
            ReservationDate = DateTime.Now.AddDays(1),
            DaysCount = 3
        };

        // Act
        var result = _service.Reserve(reserveCommandDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.DeskId);
        _reservationRepositoryMock.Verify(repo => repo.Add(It.IsAny<Reservation>()), Times.Once);
    }

    [Fact]
    public void Reserve_ShouldThrowException_WhenDeskNotFound()
    {
        // Arrange
        _deskRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Desk)null);

        var reserveCommandDto = new ReserveDeskCommandDto
        {
            DeskId = 1,
            UserId = 1,
            ReservationDate = DateTime.Now.AddDays(1),
            DaysCount = 3
        };

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => _service.Reserve(reserveCommandDto));
        Assert.Equal("Desk not found.", exception.Message);
    }
}