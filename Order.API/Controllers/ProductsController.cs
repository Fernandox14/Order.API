using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Aplicacao.DTO;
using Order.Aplicacao.Handles.Products;
using Order.Aplicacao.Handles.Reservation;

namespace Order.API.Controllers;

[ApiController]
[Route("products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retorna todos os produtos cadastrados e seus respectivos status.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<ProductDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<List<string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll(CancellationToken cancelationToken)
    {
        var response = await mediator.Send(new GetProductsQuery());
        if (response.Success)
            return Ok(response.Data);
        return NotFound(response.Errors);
    }

    /// <summary>
    /// Reserva um produto disponível para um determinado cliente.
    /// </summary>
    [HttpPost("{id:int}/reserve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<List<string>>(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> Reserve([FromRoute] int id, [FromQuery] int customerId, CancellationToken cancelationToken)
    {
        var response = await mediator.Send(new CreateReservationCommand(id, customerId));
        if (response.Success)
            return Ok();
        return BadRequest(response.Errors);
    }

    /// <summary>
    /// Cancela uma reserva ativa de um produto antes da sua expiração.
    /// </summary>
    [HttpDelete("{id:int}/reserve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<List<string>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelReservation([FromRoute] int id, CancellationToken cancelationToken)
    {
        var response = await mediator.Send(new DeleteReservationCommand(id));
        if (response.Success)
            return Ok();
        return BadRequest(response.Errors);
    }
}
