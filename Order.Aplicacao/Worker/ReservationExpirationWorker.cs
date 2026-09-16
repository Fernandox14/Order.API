using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Order.Dominio.Enums;
using Order.Dominio.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Order.Aplicacao.Worker;

public class ReservationExpirationWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<ReservationExpirationWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var reservationRepository = scope.ServiceProvider.GetRequiredService<IReservationRepository>();

                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

                var reservations = await reservationRepository.GetActiveReservationsAsync(stoppingToken);

                var now = DateTime.UtcNow;

                foreach (var reservation in reservations)
                {
                    if (!reservation.IsExpired(now)) continue;

                    reservation.Expire();

                    var product = reservation.Product!;

                    product.Status = ProductStatus.Available;

                    await reservationRepository.UpdateAsync(reservation, stoppingToken);

                    await productRepository.UpdateAsync(product, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError( ex, "Erro ao processar expiração das reservas.");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
