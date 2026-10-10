using InflightRetail.Domain.Common;

namespace InflightRetail.Domain.Entities;

public class ModifierOption : BaseEntity
{
    public Guid ModifierGroupId { get; private set; }
    public string Name { get; private set; } = null!;
    
    // Ana ürün fiyatına eklenecek/düşülecek tutar (Örn: 0.00 veya 1.50)
    public decimal PriceDelta { get; private set; }
    
    // Stoktan düşülecek fiziksel envanter kodu (isteğe bağlı)
    public string? SkuCode { get; private set; }
    
    // Ekrandaki sıralama
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation property
    public ModifierGroup ModifierGroup { get; private set; } = null!;

    private ModifierOption() { } // EF Core için

    public ModifierOption(string name, decimal priceDelta, string? skuCode = null, int displayOrder = 0)
    {
        Name = name;
        PriceDelta = priceDelta;
        SkuCode = skuCode;
        DisplayOrder = displayOrder;
    }

    public void Update(string name, decimal priceDelta, string? skuCode, int displayOrder, bool isActive)
    {
        Name = name;
        PriceDelta = priceDelta;
        SkuCode = skuCode;
        DisplayOrder = displayOrder;
        IsActive = isActive;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}