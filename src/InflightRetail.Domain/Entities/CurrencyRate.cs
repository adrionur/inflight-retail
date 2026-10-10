using InflightRetail.Domain.Common;

namespace InflightRetail.Domain.Entities;

public class CurrencyRate : BaseEntity
{
    public Guid FlightId { get; private set; }
    public string TargetCurrency { get; private set; } = null!; // TRY, USD, GBP
    public decimal RateAgainstBase { get; private set; }        // 1 EUR = ? TRY

    public Flight Flight { get; private set; } = null!;

    private CurrencyRate() { }

    public CurrencyRate(string targetCurrency, decimal rateAgainstBase)
    {
        TargetCurrency = targetCurrency;
        RateAgainstBase = rateAgainstBase;
    }
}