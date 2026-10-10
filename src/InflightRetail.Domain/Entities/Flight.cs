using InflightRetail.Domain.Common;
using InflightRetail.Domain.Enums;

namespace InflightRetail.Domain.Entities;

public class Flight : BaseEntity
{
    public string FlightNumber { get; private set; } = null!; // Örn: TK1981
    public string Origin { get; private set; } = null!;       // Örn: IST
    public string Destination { get; private set; } = null!;  // Örn: LHR
    public DateTime DepartureTimeUtc { get; private set; }
    public string AircraftType { get; private set; } = null!; // Örn: A321-NEO
    public FlightStatus Status { get; private set; } = FlightStatus.Scheduled;

    private readonly List<FlightMenuItem> _menuItems = new();
    public IReadOnlyCollection<FlightMenuItem> MenuItems => _menuItems.AsReadOnly();

    private readonly List<CurrencyRate> _currencyRates = new();
    public IReadOnlyCollection<CurrencyRate> CurrencyRates => _currencyRates.AsReadOnly();

    private Flight() { }

    public Flight(string flightNumber, string origin, string destination, DateTime departureTimeUtc, string aircraftType)
    {
        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
        DepartureTimeUtc = departureTimeUtc;
        AircraftType = aircraftType;
    }

    public void AddMenuItem(Guid productId, int upliftQuantity, decimal? overrideBasePrice = null)
    {
        _menuItems.Add(new FlightMenuItem(productId, upliftQuantity, overrideBasePrice));
    }

    public void AddCurrencyRate(string targetCurrency, decimal rate)
    {
        _currencyRates.Add(new CurrencyRate(targetCurrency, rate));
    }

    public void UpdateStatus(FlightStatus status)
    {
        Status = status;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}