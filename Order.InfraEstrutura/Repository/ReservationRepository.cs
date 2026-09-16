using Microsoft.EntityFrameworkCore;
using Order.Dominio.Entities;
using Order.Dominio.Enums;
using Order.Dominio.Interfaces.Repository;
using Order.InfraEstrutura.Database;

namespace Order.InfraEstrutura.Repository;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    private readonly AppDbContext context;

    public ReservationRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Reservation>> GetReservsByCustomerIdAsync(int customerId)
    {
        return await context.Reservations
            .Include(x => x.Product)
            .Include(x => x.Customer)
            .Where(x => x.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<Reservation?> GetByProductIdAsync(int productId, ReservationStatus productStatus)
    {
        return await DbSet
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.ProductId == productId && productStatus == x.Status);
    }

    public async Task<IEnumerable<Reservation>> GetActiveReservationsAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(x => x.Product)
            .Where(x => x.Status == ReservationStatus.Active)
            .ToListAsync(cancellationToken);
    }
}