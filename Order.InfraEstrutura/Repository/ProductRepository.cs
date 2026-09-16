using Microsoft.EntityFrameworkCore;
using Order.Dominio.Entities;
using Order.Dominio.Enums;
using Order.Dominio.Interfaces.Repository;
using Order.InfraEstrutura.Database;

namespace Order.InfraEstrutura.Repository;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Product?> GetByProductIdAsync(int productId, ProductStatus productStatus)
    {
        return await DbSet
            .FirstOrDefaultAsync(x => x.Id == productId && productStatus == x.Status);
    }
}