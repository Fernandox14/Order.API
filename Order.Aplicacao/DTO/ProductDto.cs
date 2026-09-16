using Order.Aplicacao.Helper;
using Order.Dominio.Entities;
using Order.Dominio.Enums;

namespace Order.Aplicacao.DTO;

public class ProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }

    public ProductDto(int id, string? name, decimal price, ProductStatus status)
    {
        this.Id = id;
        this.Name = name;
        this.Price = price;
        this.Status = status.ToDescription();
    }

    public ProductDto(Product product)
    {
        this.Id = product.Id;
        this.Name = product.Name;
        this.Price = product.Price;
        this.Status = product.Status.ToDescription();
    }

    public ProductDto()
    {
        
    }
}
