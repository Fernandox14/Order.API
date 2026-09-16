using Order.Dominio.Enums;

namespace Order.Dominio.Entities;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public ProductStatus Status { get; set; }
}
