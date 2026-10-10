using InflightRetail.Domain.Common;

namespace InflightRetail.Domain.Entities;

public class Product : BaseEntity
{
    public string Sku { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public decimal BasePrice { get; private set; }
    public string CurrencyCode { get; private set; } = "EUR"; // Uçak içi varsayılan baz para birimi
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public ICollection<ProductModifierGroup> ProductModifierGroups { get; private set; } = new List<ProductModifierGroup>();

    private Product() { }

    public Product(string sku, string name, decimal basePrice, string currencyCode = "EUR")
    {
        Sku = sku;
        Name = name;
        BasePrice = basePrice;
        CurrencyCode = currencyCode;
    }
}