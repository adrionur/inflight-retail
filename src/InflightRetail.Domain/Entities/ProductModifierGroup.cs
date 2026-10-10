namespace InflightRetail.Domain.Entities;

public class ProductModifierGroup
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public Guid ModifierGroupId { get; set; }
    public ModifierGroup ModifierGroup { get; set; } = null!;

    // Bu ürün özelinde bu grubun gösterim sırası
    public int DisplayOrder { get; set; }
}