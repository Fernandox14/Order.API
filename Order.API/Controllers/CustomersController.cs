using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Aplicacao.DTO;
using Order.Aplicacao.Handles.Customer;

namespace Order.API.Controllers;

[ApiController]
[Route("customer")]
public class CustomersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retorna a lista de produtos que o cliente reservou.
    /// </summary>
    [HttpGet("{id_customer:int}/reservations")]
    [ProducesResponseType<IEnumerable<ReservationDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<List<string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerReservations([FromRoute] int id_Customer, CancellationToken cancelationToken)
    {
        var response = await mediator.Send(new GetProductsByCustomerQuery(id_Customer));
        if (response.Success)
            return Ok(response.Data);
        return NotFound(response.Errors);
    }
}