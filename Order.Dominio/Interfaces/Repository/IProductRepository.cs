using Order.Dominio.Entities;
using Order.Dominio.Enums;

namespace Order.Dominio.Interfaces.Repository;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetByProductIdAsync(int productId, ProductStatus productStatus);
}
