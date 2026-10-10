using InflightRetail.Domain.Common;

namespace InflightRetail.Domain.Entities;

public class ModifierGroup : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    
    // Seçim kuralları:
    // Örn: Min=1, Max=1 -> Zorunlu Tekli Seçim (Radio button)
    // Örn: Min=0, Max=3 -> İsteğe Bağlı Çoklu Seçim (Checkbox)
    public int MinSelection { get; private set; }
    public int MaxSelection { get; private set; }
    
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    private readonly List<ModifierOption> _options = new();
    public IReadOnlyCollection<ModifierOption> Options => _options.AsReadOnly();

    public ICollection<ProductModifierGroup> ProductModifierGroups { get; private set; } = new List<ProductModifierGroup>();

    private ModifierGroup() { }

    public ModifierGroup(string name, int minSelection, int maxSelection, string? description = null)
    {
        if (minSelection < 0) throw new ArgumentException("MinSelection 0'dan küçük olamaz.", nameof(minSelection));
        if (maxSelection < minSelection) throw new ArgumentException("MaxSelection, MinSelection'dan küçük olamaz.", nameof(maxSelection));

        Name = name;
        MinSelection = minSelection;
        MaxSelection = maxSelection;
        Description = description;
    }

    public void AddOption(ModifierOption option)
    {
        _options.Add(option);
    }
}