using MediatR;
using Order.Aplicacao.Dto;
using Order.Dominio.Enums;
using Order.Dominio.Interfaces.Repository;

namespace Order.Aplicacao.Handles.Reservation;

public class DeleteReservationCommandHandler(
    IReservationRepository reservationRepository,
    IProductRepository productRepository) : IRequestHandler<DeleteReservationCommand, BaseResponseDto<DeleteReservationCommandResponse>>
{
    public async Task<BaseResponseDto<DeleteReservationCommandResponse>> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponseDto<DeleteReservationCommandResponse>();

        try
        {
            var reserve = await reservationRepository.GetByIdAsync(request.ReservationId);

            if (reserve == null)
            {
                throw new Exception($"Reserva {request.ReservationId} não encontrado.");
            }

            if (reserve.Status == ReservationStatus.Cancelled)
            {
                throw new Exception($"Reserva {request.ReservationId} já se encontra cancelada.");
            }

            reserve.Status = ReservationStatus.Cancelled;

            await reservationRepository.UpdateAsync(reserve, cancellationToken);

            var product = await productRepository.GetByProductIdAsync(reserve.ProductId, ProductStatus.Reserved);
            if (product == null)
            {
                throw new Exception($"Produto {reserve.ProductId} não encontrado.");
            }

            product.Status = ProductStatus.Available;

            await productRepository.UpdateAsync(product);

            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Errors = new List<string> { ex.Message };
            response.Success = false;
        }

        return response;
    }
}
