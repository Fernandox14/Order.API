using Order.Dominio.Entities;
using Order.Dominio.Interfaces.Repository;
using Order.InfraEstrutura.Database;

namespace Order.InfraEstrutura.Repository;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context): base(context)
    {
    }
}
