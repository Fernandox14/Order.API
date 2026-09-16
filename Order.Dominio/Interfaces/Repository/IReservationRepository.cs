using Order.Dominio.Entities;
using Order.Dominio.Enums;

namespace Order.Dominio.Interfaces.Repository;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    Task<IEnumerable<Reservation>> GetReservsByCustomerIdAsync(int customerId);
    Task<Reservation?> GetByProductIdAsync(int productId, ReservationStatus productStatus);

    Task<IEnumerable<Reservation>> GetActiveReservationsAsync(CancellationToken cancellationToken = default);
}
