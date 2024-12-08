
using Microsoft.EntityFrameworkCore;

namespace LoadDwhVenta.Models.Models;

public partial class dwventasContext : DbContext
{
    public dwventasContext(DbContextOptions<dwventasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FactClientesAtendido> FactClientesAtendidos { get; set; }

    public virtual DbSet<FactOrder> FactOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FactClientesAtendido>(entity =>
        {
            entity.HasKey(e => e.ClienteAtendidoKey).HasName("PK__FactClie__B70C6B93F884893C");
        });

        modelBuilder.Entity<FactOrder>(entity =>
        {
            entity.HasKey(e => e.OrderNumber).HasName("PK__FactOrde__CAC5E742EDA0F446");

            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalVentas).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}