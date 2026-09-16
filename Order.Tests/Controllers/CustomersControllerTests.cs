using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Order.API.Controllers;
using Order.Aplicacao.Dto;
using Order.Aplicacao.DTO;
using Order.Aplicacao.Handles.Customer;

namespace Order.Tests.Controllers;

public class CustomersControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly CustomersController _controller;

    public CustomersControllerTests()
    {
        _mediator = new Mock<IMediator>();

        _controller = new CustomersController(
            _mediator.Object);
    }

    [Fact]
    public async Task GetCustomerReservations_Should_Return_Ok()
    {
        var customerId = 1;

        var reservations = new List<ReservationDto>
        {
            new ReservationDto
            {
                ReservationId = 1,
                ProductId = 10,
                ProductName = "iPhone 16"
            }
        };

        var response = new BaseResponseDto<IEnumerable<ReservationDto>>
        {
            Success = true,
            Data = reservations
        };

        _mediator
            .Setup(x => x.Send(
                It.Is<GetProductsByCustomerQuery>(
                    q => q.CustomerId == customerId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.GetCustomerReservations(customerId, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(reservations, okResult.Value);

        _mediator.Verify(
            x => x.Send(
                It.Is<GetProductsByCustomerQuery>(
                    q => q.CustomerId == customerId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
