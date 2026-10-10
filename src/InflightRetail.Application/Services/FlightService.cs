using InflightRetail.Application.Common.Interfaces;
using InflightRetail.Application.DTOs;
using InflightRetail.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InflightRetail.Application.Services;

public class FlightService : IFlightService
{
    private readonly IAppDbContext _context;

    public FlightService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<FlightSummaryDto> CreateFlightAsync(CreateFlightDto dto, CancellationToken ct = default)
    {
        var flight = new Flight(
            dto.FlightNumber,
            dto.Origin,
            dto.Destination,
            dto.DepartureTimeUtc,
            dto.AircraftType
        );

        foreach (var item in dto.MenuItems)
        {
            flight.AddMenuItem(item.ProductId, item.UpliftQuantity, item.OverrideBasePrice);
        }

        foreach (var rate in dto.CurrencyRates)
        {
            flight.AddCurrencyRate(rate.TargetCurrency, rate.RateAgainstBase);
        }

        _context.Flights.Add(flight);
        await _context.SaveChangesAsync(ct);

        return new FlightSummaryDto(
            flight.Id,
            flight.FlightNumber,
            $"{flight.Origin} -> {flight.Destination}",
            flight.DepartureTimeUtc,
            flight.AircraftType,
            flight.Status,
            flight.MenuItems.Count
        );
    }

    public async Task<List<FlightSummaryDto>> GetFlightsAsync(CancellationToken ct = default)
    {
        return await _context.Flights
            .AsNoTracking()
            .Select(f => new FlightSummaryDto(
                f.Id,
                f.FlightNumber,
                $"{f.Origin} -> {f.Destination}",
                f.DepartureTimeUtc,
                f.AircraftType,
                f.Status,
                f.MenuItems.Count
            ))
            .ToListAsync(ct);
    }

    public async Task<FlightManifestDto?> GetFlightManifestAsync(Guid flightId, CancellationToken ct = default)
    {
        var flight = await _context.Flights
            .AsNoTracking()
            .Include(f => f.CurrencyRates)
            .Include(f => f.MenuItems)
                .ThenInclude(mi => mi.Product)
                    .ThenInclude(p => p.ProductModifierGroups)
                        .ThenInclude(pmg => pmg.ModifierGroup)
                            .ThenInclude(mg => mg.Options)
            .FirstOrDefaultAsync(f => f.Id == flightId, ct);

        if (flight == null) return null;

        var exchangeRates = flight.CurrencyRates
            .ToDictionary(r => r.TargetCurrency.ToUpperInvariant(), r => r.RateAgainstBase);

        var menuCatalog = flight.MenuItems
            .Where(mi => mi.Product.IsActive)
            .Select(mi =>
            {
                var product = mi.Product;
                var effectivePrice = mi.OverrideBasePrice ?? product.BasePrice;

                var modifierGroups = product.ProductModifierGroups
                    .Where(pmg => pmg.ModifierGroup.IsActive)
                    .OrderBy(pmg => pmg.DisplayOrder)
                    .Select(pmg => new ManifestModifierGroupDto(
                        pmg.ModifierGroup.Id,
                        pmg.ModifierGroup.Name,
                        pmg.ModifierGroup.MinSelection,
                        pmg.ModifierGroup.MaxSelection,
                        pmg.DisplayOrder,
                        pmg.ModifierGroup.Options
                            .Where(o => o.IsActive)
                            .OrderBy(o => o.DisplayOrder)
                            .Select(o => new ManifestProductOptionDto(
                                o.Id,
                                o.Name,
                                o.PriceDelta,
                                o.SkuCode,
                                o.DisplayOrder
                            )).ToList()
                    )).ToList();

                return new ManifestItemDto(
                    product.Id,
                    product.Sku,
                    product.Name,
                    effectivePrice,
                    product.CurrencyCode,
                    mi.UpliftQuantity,
                    modifierGroups
                );
            })
            .ToList();

        return new FlightManifestDto(
            flight.Id,
            flight.FlightNumber,
            flight.Origin,
            flight.Destination,
            flight.DepartureTimeUtc,
            "EUR", // Base Currency
            exchangeRates,
            menuCatalog
        );
    }
}