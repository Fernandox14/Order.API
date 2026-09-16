using Order.Dominio.Enums;

namespace Order.Dominio.Entities;

public class Reservation
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public Customer? Customer { get; set; }
    public Product? Product { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public ReservationStatus Status { get; set; }

    public bool IsExpired(DateTime utcNow)
    {
        return utcNow >= ExpiresAtUtc;
    }

    public void Expire()
    {
        Status = ReservationStatus.Expired;
    }
}
