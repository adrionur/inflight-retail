using System.Reflection;
using InflightRetail.Application.Common.Interfaces;
using InflightRetail.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InflightRetail.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // IAppDbContext arayüzünden gelen DbSet tanımları
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ModifierGroup> ModifierGroups => Set<ModifierGroup>();
    public DbSet<ModifierOption> ModifierOptions => Set<ModifierOption>();
    public DbSet<ProductModifierGroup> ProductModifierGroups => Set<ProductModifierGroup>();

    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<FlightMenuItem> FlightMenuItems => Set<FlightMenuItem>();
    public DbSet<CurrencyRate> CurrencyRates => Set<CurrencyRate>();

    // DbContext'in kendi SaveChangesAsync metodu IAppDbContext'teki imzayla birebir örtüşür
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Assembly içindeki Fluent API konfigürasyonlarını otomatik uygular
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}