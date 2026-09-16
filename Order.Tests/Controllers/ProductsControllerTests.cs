using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Order.API.Controllers;
using Order.Aplicacao.Dto;
using Order.Aplicacao.DTO;
using Order.Aplicacao.Handles.Products;
using Order.Aplicacao.Handles.Reservation;
using Order.Aplicacao.Helper;
using Order.Dominio.Entities;
using Order.Dominio.Enums;

namespace Order.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new ProductsController(_mediator.Object);
    }

    [Fact]
    public async Task GetAll_Should_Return_Ok_When_Success()
    {
        var products = new List<ProductDto>
        {
            new ProductDto
            {
                Id = 1,
                Name = "iPhone 16",
                Price = 6299,
                Status = ProductStatus.Available.ToDescription()
            },
            new ProductDto
            {
                Id = 2,
                Name = "Notebook Dell",
                Price = 4500,
                Status = ProductStatus.Available.ToDescription()
            }
        };

        var response = new BaseResponseDto<IEnumerable<ProductDto>>
        {
            Success = true,
            Data = products
        };

        _mediator
            .Setup(x => x.Send(
                It.IsAny<GetProductsQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.GetAll(CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(products, okResult.Value);

        _mediator.Verify(
            x => x.Send(
                It.IsAny<GetProductsQuery>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAll_Should_Return_NotFound_When_Fails()
    {
        var errors = new List<string>
        {
            "Nenhum produto encontrado."
        };

        var response = new BaseResponseDto<IEnumerable<ProductDto>>
        {
            Success = false,
            Errors = errors
        };

        _mediator
            .Setup(x => x.Send(
                It.IsAny<GetProductsQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.GetAll(CancellationToken.None);

        var notFoundResult =
            Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal(errors, notFoundResult.Value);
    }

    [Fact]
    public async Task Reserve_Should_Return_Ok_When_Success()
    {
        var productId = 1;
        var customerId = 10;

        var response = new BaseResponseDto<CreateReservationCommandResponse>
        {
            Success = true
        };

        _mediator
            .Setup(x => x.Send(
                It.Is<CreateReservationCommand>(
                    x => x.ProductId == productId &&
                         x.CustomerId == customerId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.Reserve(
            productId,
            customerId,
            CancellationToken.None);

        Assert.IsType<OkResult>(result);

        _mediator.Verify(
            x => x.Send(
                It.Is<CreateReservationCommand>(
                    x => x.ProductId == productId &&
                         x.CustomerId == customerId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Reserve_Should_Return_BadRequest_When_Fails()
    {
        // Arrange
        var productId = 1;
        var customerId = 10;

        var errors = new List<string>
        {
            "Produto não está mais disponível."
        };

        var response =
            new BaseResponseDto<CreateReservationCommandResponse>
            {
                Success = false,
                Errors = errors
            };

        _mediator
            .Setup(x => x.Send(
                It.Is<CreateReservationCommand>(
                    x => x.ProductId == productId &&
                         x.CustomerId == customerId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.Reserve(
            productId,
            customerId,
            CancellationToken.None);

        var badRequestResult =
            Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal(errors, badRequestResult.Value);
    }

    [Fact]
    public async Task CancelReservation_Should_Return_Ok_When_Success()
    {
        var reservationId = 1;

        var response = new BaseResponseDto<DeleteReservationCommandResponse>
        {
            Success = true,
            Data = new DeleteReservationCommandResponse
            {
                IsDeleted = true,
                ReservationDate = DateTime.UtcNow,
            }
        };

        _mediator
            .Setup(x => x.Send(
                It.Is<DeleteReservationCommand>(
                    x => x.ReservationId == reservationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.CancelReservation(reservationId,CancellationToken.None);

        Assert.IsType<OkResult>(result);

        _mediator.Verify(
            x => x.Send(
                It.Is<DeleteReservationCommand>(
                    x => x.ReservationId == reservationId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CancelReservation_Should_Return_BadRequest_When_Fails()
    {
        var reservationId = 1;

        var errors = new List<string>
        {
            "Reserva não encontrada."
        };

        var response = new BaseResponseDto<DeleteReservationCommandResponse>
        {
            Success = false,
            Errors = errors
        };

        _mediator
            .Setup(x => x.Send(
                It.Is<DeleteReservationCommand>(
                    x => x.ReservationId == reservationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.CancelReservation(reservationId,CancellationToken.None);

        var badRequestResult =
            Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal(errors, badRequestResult.Value);
    }
}