using MediatR;
using Order.Aplicacao.Dto;

namespace Order.Aplicacao.Handles.Reservation;

public class CreateReservationCommand : IRequest<BaseResponseDto<CreateReservationCommandResponse>>
{
    public int ProductId { get; set; }
    public int CustomerId { get; set; }

    public CreateReservationCommand(int ProductId, int CustomerId)
    {
        this.ProductId = ProductId;
        this.CustomerId = CustomerId;
    }
}