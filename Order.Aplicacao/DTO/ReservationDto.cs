using Order.Dominio.Entities;
using Order.Dominio.Enums;
using Order.Aplicacao.Helper;
namespace Order.Aplicacao.DTO;

public class ReservationDto
{
    public int ReservationId { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public string Status { get; set; }

    public ReservationDto(Reservation reservation)
    {
        this.ReservationId = reservation.Id;
        this.ProductId = reservation.ProductId;
        this.ProductName = reservation.Product?.Name;
        this.ProductPrice = reservation.Product!.Price;
        this.CreatedAtUtc = reservation.CreatedAtUtc;
        this.ExpiresAtUtc = reservation.ExpiresAtUtc;
        this.Status = reservation.Status.ToDescription();
    }

    public ReservationDto()
    {
        
    }
}