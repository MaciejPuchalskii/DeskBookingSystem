using DeskBookingSystem.Data.Models;

public class ReservationRepositoryTests
{
    private readonly BookingContext _context;
    private readonly ReservationRepository _repository;

    public ReservationRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<BookingContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new BookingContext(options);
        _repository = new ReservationRepository(_context);
    }

    [Fact]
    public void Add_ShouldAddReservation()
    {
        // Arrange
        var reservation = new Reservation
        {
            DeskId = 1,
            UserId = 1,
            ReservationDate = DateTime.Now,
            DaysCount = 3
        };

        // Act
        _repository.Add(reservation);
        var result = _context.Reservations.FirstOrDefault(r => r.Id == reservation.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(reservation.DeskId, result.DeskId);
        Assert.Equal(reservation.UserId, result.UserId);
    }

    [Fact]
    public void GetById_ShouldReturnReservation_WhenExists()
    {
        // Arrange
        var reservation = new Reservation
        {
            DeskId = 2,
            UserId = 2,
            ReservationDate = DateTime.Now,
            DaysCount = 2
        };

        _context.Reservations.Add(reservation);
        _context.SaveChanges();

        // Act
        var result = _repository.GetById(reservation.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(reservation.Id, result.Id);
        Assert.Equal(reservation.DeskId, result.DeskId);
    }

    [Fact]
    public void GetAllReservationsByDeskId_ShouldReturnReservations_WhenExist()
    {
        // Arrange
        var reservation1 = new Reservation
        {
            DeskId = 1,
            UserId = 1,
            ReservationDate = DateTime.Now,
            DaysCount = 1
        };

        var reservation2 = new Reservation
        {
            DeskId = 1,
            UserId = 2,
            ReservationDate = DateTime.Now.AddDays(2),
            DaysCount = 2
        };

        _context.Reservations.AddRange(reservation1, reservation2);
        _context.SaveChanges();

        // Act
        var result = _repository.GetAllByDeskId(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Update_ShouldModifyExistingReservation()
    {
        // Arrange
        var reservation = new Reservation
        {
            DeskId = 1,
            UserId = 1,
            ReservationDate = DateTime.Now,
            DaysCount = 3
        };

        _context.Reservations.Add(reservation);
        _context.SaveChanges();

        // Act
        var existingReservation = _context.Reservations.Find(reservation.Id);
        existingReservation.DeskId = 2;
        existingReservation.ReservationDate = DateTime.Now.AddDays(1);
        existingReservation.DaysCount = 5;

        // Act
        _repository.Update(existingReservation);
        var result = _context.Reservations.Find(reservation.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.DeskId);
        Assert.Equal(5, result.DaysCount);
    }

    [Fact]
    public void UpdateReservation_ShouldModifyReservationDetails()
    {
        // Arrange
        var reservation = new Reservation
        {
            DeskId = 1,
            UserId = 1,
            ReservationDate = DateTime.Now,
            DaysCount = 3
        };

        _context.Reservations.Add(reservation);
        _context.SaveChanges();

        // Act
        var newDate = DateTime.Now.AddDays(2);
        _repository.UpdateReservation(reservation, 4, newDate);
        var result = _context.Reservations.Find(reservation.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newDate, result.ReservationDate);
        Assert.Equal(4, result.DaysCount);
    }
}