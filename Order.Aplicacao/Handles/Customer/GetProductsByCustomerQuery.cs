using MediatR;
using Order.Aplicacao.Dto;
using Order.Aplicacao.DTO;

namespace Order.Aplicacao.Handles.Customer;

public class GetProductsByCustomerQuery : IRequest<BaseResponseDto<IEnumerable<ReservationDto>>>
{
    public int CustomerId { get; set; }

    public GetProductsByCustomerQuery(int CustomerId)
    {
        this.CustomerId = CustomerId;
    }
}