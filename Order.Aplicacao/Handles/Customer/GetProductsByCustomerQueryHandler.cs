using MediatR;
using Order.Aplicacao.Dto;
using Order.Aplicacao.DTO;
using Order.Dominio.Interfaces.Repository;

namespace Order.Aplicacao.Handles.Customer;

public class GetProductsByCustomerQueryHandler(
    IReservationRepository reservationRepository,
    ICustomerRepository customerRepository) : IRequestHandler<GetProductsByCustomerQuery, BaseResponseDto<IEnumerable<ReservationDto>>>
{
    public async Task<BaseResponseDto<IEnumerable<ReservationDto>>> Handle(GetProductsByCustomerQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponseDto<IEnumerable<ReservationDto>>();

        try
        {
            var customer = await customerRepository.GetByIdAsync(request.CustomerId);

            if (customer == null)
            {
                throw new Exception($"Cliente {request.CustomerId} não encontrado.");
            }

            var reservs = await reservationRepository.GetReservsByCustomerIdAsync(customer.Id);
            response.Success = true;
            response.Data = reservs.Select(reserv => new ReservationDto(reserv));
        }
        catch (Exception ex)
        {
            response.Errors = new List<string> { ex.Message };
            response.Success = false;
        }

        return response;
    }
}