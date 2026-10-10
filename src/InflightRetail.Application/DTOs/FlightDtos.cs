using InflightRetail.Domain.Enums;

namespace InflightRetail.Application.DTOs;

// --- Uçuş Oluşturma & Yönetim DTO'ları ---
public record CreateFlightItemDto(
    Guid ProductId,
    int UpliftQuantity,
    decimal? OverrideBasePrice
);

public record CreateCurrencyRateDto(
    string TargetCurrency,
    decimal RateAgainstBase
);

public record CreateFlightDto(
    string FlightNumber,
    string Origin,
    string Destination,
    DateTime DepartureTimeUtc,
    string AircraftType,
    List<CreateFlightItemDto> MenuItems,
    List<CreateCurrencyRateDto> CurrencyRates
);

public record FlightSummaryDto(
    Guid Id,
    string FlightNumber,
    string Route,
    DateTime DepartureTimeUtc,
    string AircraftType,
    FlightStatus Status,
    int TotalItemCount
);

// --- Mobil Tabletin Çekeceği Optimize Manifest Paketi ---
public record ManifestProductOptionDto(
    Guid Id,
    string Name,
    decimal PriceDelta,
    string? SkuCode,
    int DisplayOrder
);

public record ManifestModifierGroupDto(
    Guid Id,
    string Name,
    int MinSelection,
    int MaxSelection,
    int DisplayOrder,
    List<ManifestProductOptionDto> Options
);

public record ManifestItemDto(
    Guid ProductId,
    string Sku,
    string Name,
    decimal EffectivePrice, // Override edildiyse o, yoksa BasePrice
    string BaseCurrency,
    int AvailableStock,     // Uçuşa yüklenen miktar
    List<ManifestModifierGroupDto> ModifierGroups
);

public record FlightManifestDto(
    Guid FlightId,
    string FlightNumber,
    string Origin,
    string Destination,
    DateTime DepartureTimeUtc,
    string BaseCurrency,
    Dictionary<string, decimal> ExchangeRates, // Örn: { "TRY": 38.50, "USD": 1.08, "GBP": 0.84 }
    List<ManifestItemDto> MenuCatalog
);