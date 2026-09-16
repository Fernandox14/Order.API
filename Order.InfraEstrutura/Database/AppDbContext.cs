using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Order.Dominio.Entities;
using Order.Dominio.Enums;

namespace Order.InfraEstrutura.Database;


public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Product>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Reservation>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Reservation>()
            .HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId);

        modelBuilder.Entity<Reservation>()
            .HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId);

        SetProducts(modelBuilder);
        SetCustomers(modelBuilder);
    }

    private static void SetProducts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new
            {
                Id = 1,
                Name = "Notebook Dell Inspiron",
                Price = 4599.90m,
                Status = ProductStatus.Available
            },
            new
            {
                Id = 2,
                Name = "iPhone 16",
                Price = 6299.00m,
                Status = ProductStatus.Available
            },
            new
            {
                Id = 3,
                Name = "Monitor LG UltraWide 29",
                Price = 1899.90m,
                Status = ProductStatus.Available
            },
            new
            {
                Id = 4,
                Name = "Headset HyperX Cloud III",
                Price = 799.90m,
                Status = ProductStatus.Available
            },
            new
            {
                Id = 5,
                Name = "Teclado Mecânico Logitech",
                Price = 549.90m,
                Status = ProductStatus.Available
            }
        );
    }

    private static void SetCustomers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasData(
            new
            {
                Id = 1,
                Name = "Fernando Silva",
                Email = "fernando@email.com"
            },
            new
            {
                Id = 2,
                Name = "João Santos",
                Email = "joao@email.com"
            },
            new
            {
                Id = 3,
                Name = "Maria Oliveira",
                Email = "maria@email.com"
            }
        );
    }
}