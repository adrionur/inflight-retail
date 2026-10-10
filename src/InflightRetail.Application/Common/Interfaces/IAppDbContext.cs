using InflightRetail.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InflightRetail.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Product> Products { get; }
    DbSet<ModifierGroup> ModifierGroups { get; }
    DbSet<ModifierOption> ModifierOptions { get; }
    DbSet<ProductModifierGroup> ProductModifierGroups { get; }

    DbSet<Flight> Flights { get; }
    DbSet<FlightMenuItem> FlightMenuItems { get; }
    DbSet<CurrencyRate> CurrencyRates { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}