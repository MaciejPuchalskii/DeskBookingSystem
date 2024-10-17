public class ReservationControllerTests
{
    private readonly Mock<IReservationService> _reservationServiceMock;
    private readonly ReservationController _controller;

    public ReservationControllerTests()
    {
        _reservationServiceMock = new Mock<IReservationService>();
        _controller = new ReservationController(_reservationServiceMock.Object);
    }

    [Fact]
    public void ChangeReservationDate_ShouldReturnOk_WhenReservationIsChanged()
    {
        // Arrange
        var changeReservationDateResponse = new ChangeReservationDateResponseDto
        {
            ReservationId = 1,
            DeskId = 1,
            UserId = 1,
            startDate = DateTime.Now,
            endDate = DateTime.Now.AddDays(2)
        };

        _reservationServiceMock.Setup(service => service.ChangeReservationDate(It.IsAny<ChangeReservationDateCommandDto>())).Returns(changeReservationDateResponse);

        // Act
        var result = _controller.ChangeReservationDate(1, 1, DateTime.Now, 2);

        // Assert
        var okResult = Assert.IsType<ActionResult<ChangeReservationDateResponseDto>>(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);

        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(changeReservationDateResponse, okObjectResult.Value);
    }

    [Fact]
    public void ChangeReservationDate_ShouldReturnNotFound_WhenReservationNotFound()
    {
        // Arrange
        _reservationServiceMock.Setup(service => service.ChangeReservationDate(It.IsAny<ChangeReservationDateCommandDto>())).Throws(new Exception("Reservation not found."));

        // Act
        var result = _controller.ChangeReservationDate(1, 1, DateTime.Now, 2);

        // Assert
        var notFoundResult = Assert.IsType<ActionResult<ChangeReservationDateResponseDto>>(result);
        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        Assert.Equal(404, notFoundObjectResult.StatusCode);
        Assert.Equal("Reservation not found.", notFoundObjectResult.Value);
    }

    [Fact]
    public void ChangeReservationDesk_ShouldReturnOk_WhenDeskIsChanged()
    {
        // Arrange
        var changeDeskResponse = new ChangeReservationDeskResponseDto
        {
            ReservationId = 1,
            DeskId = 2
        };

        _reservationServiceMock.Setup(service => service.ChangeReservationDesk(It.IsAny<ChangeReservationDeskCommandDto>())).Returns(changeDeskResponse);

        // Act
        var result = _controller.ChangeReservationDesk(1, 2);

        // Assert
        var okResult = Assert.IsType<ActionResult<ChangeReservationDeskResponseDto>>(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(changeDeskResponse, okObjectResult.Value);
    }

    [Fact]
    public void ReserveDesk_ShouldReturnBadRequest_WhenDeskIsNotAvailable()
    {
        // Arrange
        var reserveDeskCommandDto = new ReserveDeskCommandDto
        {
            DeskId = 1,
            UserId = 1,
            ReservationDate = DateTime.Now,
            DaysCount = 3
        };

        _reservationServiceMock.Setup(service => service.Reserve(It.IsAny<ReserveDeskCommandDto>())).Throws(new Exception("Desk is not available for reservation."));

        // Act
        var result = _controller.ReserveDesk(reserveDeskCommandDto);

        // Assert
        var badRequestResult = Assert.IsType<ActionResult<ReserveDeskResponseDto>>(result);
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
        Assert.Equal(400, badRequestObjectResult.StatusCode);
        Assert.Equal("Desk is not available for reservation.", badRequestObjectResult.Value);
    }
}