using Microsoft.EntityFrameworkCore;
using PowerDNSExpert.Data.Entities;

namespace PowerDNSExpert.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(builder =>
        {
            builder.ToTable("products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.Price).HasColumnType("numeric(12,2)");
        });

        modelBuilder.Entity<Order>(builder =>
        {
            builder.ToTable("orders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CustomerEmail).HasMaxLength(256).IsRequired();
            builder.HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(builder =>
        {
            builder.ToTable("order_items");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UnitPrice).HasColumnType("numeric(12,2)");
        });
    }
}
