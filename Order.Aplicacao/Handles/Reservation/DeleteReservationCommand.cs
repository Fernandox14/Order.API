using MediatR;
using Order.Aplicacao.Dto;

namespace Order.Aplicacao.Handles.Reservation;

public class DeleteReservationCommand : IRequest<BaseResponseDto<DeleteReservationCommandResponse>>
{
    public int ReservationId { get; set; }

    public DeleteReservationCommand(int ReservationId)
    {
        this.ReservationId = ReservationId;
    }
}
