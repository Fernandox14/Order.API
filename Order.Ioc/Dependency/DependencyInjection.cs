using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Aplicacao.Handles.Reservation;
using Order.Aplicacao.Worker;
using Order.Dominio.Interfaces.Repository;
using Order.InfraEstrutura.Database;
using Order.InfraEstrutura.Repository;
using MediatR;
namespace Order.Ioc.Dependency;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(CreateReservationCommand).Assembly);

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("DbOrder");
        });

      
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddHostedService<ReservationExpirationWorker>();

        return services;
    }
}
