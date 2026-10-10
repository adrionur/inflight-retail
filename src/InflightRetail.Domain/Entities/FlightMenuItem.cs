using InflightRetail.Domain.Common;

namespace InflightRetail.Domain.Entities;

public class FlightMenuItem : BaseEntity
{
    public Guid FlightId { get; private set; }
    public Guid ProductId { get; private set; }
    
    // Catering firmasının uçağa yüklediği fiziksel adet
    public int UpliftQuantity { get; private set; }
    
    // Satış sonrası kalan adet (Uçuş tamamlanınca hesaplanır)
    public int RemainingQuantity { get; private set; }
    
    // Uçuş özelinde ürünün taban fiyatı ezilebilir (opsiyonel)
    public decimal? OverrideBasePrice { get; private set; }

    public Flight Flight { get; private set; } = null!;
    public Product Product { get; private set; } = null!;

    private FlightMenuItem() { }

    public FlightMenuItem(Guid productId, int upliftQuantity, decimal? overrideBasePrice = null)
    {
        ProductId = productId;
        UpliftQuantity = upliftQuantity;
        RemainingQuantity = upliftQuantity;
        OverrideBasePrice = overrideBasePrice;
    }

    public void UpdateRemaining(int remaining)
    {
        RemainingQuantity = remaining;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}