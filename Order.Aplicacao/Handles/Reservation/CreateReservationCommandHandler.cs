using MediatR;
using Order.Aplicacao.Dto;
using Order.Aplicacao.DTO;
using Order.Dominio.Enums;
using Order.Dominio.Interfaces.Repository;
using System.Collections.Concurrent;

namespace Order.Aplicacao.Handles.Reservation;

public class CreateReservationCommandHandler(
    IProductRepository productRepository,
    IReservationRepository reservationRepository,
    ICustomerRepository customerRepository)
    : IRequestHandler<
        CreateReservationCommand,
        BaseResponseDto<CreateReservationCommandResponse>>
{
    private static readonly ConcurrentDictionary<int, SemaphoreSlim> ProductLocks = new();

    public async Task<BaseResponseDto<CreateReservationCommandResponse>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponseDto<CreateReservationCommandResponse>();

        var productLock = ProductLocks.GetOrAdd(request.ProductId, _ => new SemaphoreSlim(1, 1));

        await productLock.WaitAsync(cancellationToken);

        try
        {
            var product = await productRepository .GetByIdAsync(request.ProductId);

            if (product == null)
            {
                throw new Exception( $"Produto {request.ProductId} não encontrado.");
            }

            var reservation = await reservationRepository.GetByProductIdAsync(product.Id, ReservationStatus.Active);
            if (reservation != null && (reservation.IsExpired(DateTime.UtcNow)))
            {
                product.Status = ProductStatus.Available;
                reservation.Status = ReservationStatus.Expired;

                await reservationRepository.UpdateAsync(reservation);
                await productRepository.UpdateAsync(product);
            }

            if (product.Status != ProductStatus.Available)
            {
                throw new Exception($"Produto {product.Name} não está mais disponível.");
            }

            var customer = await customerRepository.GetByIdAsync(request.CustomerId);

            if (customer == null)
            {
                throw new Exception( $"Cliente {request.CustomerId} não encontrado.");
            }

            var now = DateTime.UtcNow;

            var newReservation = new Dominio.Entities.Reservation
            {
                CustomerId = customer.Id,
                Customer = customer,

                ProductId = product.Id,
                Product = product,

                CreatedAtUtc = now,
                ExpiresAtUtc = now.AddHours(72),

                Status = ReservationStatus.Active
            };

            product.Status = ProductStatus.Reserved;

            await reservationRepository.AddAsync(newReservation, cancellationToken);

            await productRepository.UpdateAsync(product);

            response.Success = true;

            response.Data = new CreateReservationCommandResponse
            {
                IsCreated = true,
                ReservationDate = now,
            };
        }
        catch (Exception ex)
        {
            response.Errors = new List<string> { ex.Message };
            response.Success = false;
        }
        finally
        {
            productLock.Release();
        }

        return response;
    }
}
