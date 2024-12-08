
using Microsoft.EntityFrameworkCore;

namespace LoadDwhVenta.Models.Models;

public partial class NorthwindContext : DbContext
{
    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }

    public virtual DbSet<VwServedCustomer> VwServedCustomers { get; set; }

    public virtual DbSet<VwVenta> VwVentas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VwServedCustomer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwServedCustomer", "DWH");

            entity.Property(e => e.ClienteAtendidoKey)
                .IsRequired()
                .HasMaxLength(5)
                .IsFixedLength();
        });

        modelBuilder.Entity<VwVenta>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VWVentas", "DWH");

            entity.Property(e => e.CategoryName)
                .IsRequired()
                .HasMaxLength(15);
            entity.Property(e => e.Country).HasMaxLength(15);
            entity.Property(e => e.CustomerId)
                .IsRequired()
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeName)
                .IsRequired()
                .HasMaxLength(31);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.Shipper)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.ShipperId).HasColumnName("ShipperID");
            entity.Property(e => e.TotalVentas).HasColumnType("money");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}