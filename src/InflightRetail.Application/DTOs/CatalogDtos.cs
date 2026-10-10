namespace InflightRetail.Application.DTOs;

// --- Modifier Option DTOs ---
public record CreateModifierOptionDto(
    string Name,
    decimal PriceDelta,
    string? SkuCode,
    int DisplayOrder
);

public record ModifierOptionDto(
    Guid Id,
    string Name,
    decimal PriceDelta,
    string? SkuCode,
    int DisplayOrder,
    bool IsActive
);

// --- Modifier Group DTOs ---
public record CreateModifierGroupDto(
    string Name,
    string? Description,
    int MinSelection,
    int MaxSelection,
    List<CreateModifierOptionDto> Options
);

public record ModifierGroupDto(
    Guid Id,
    string Name,
    string? Description,
    int MinSelection,
    int MaxSelection,
    bool IsActive,
    List<ModifierOptionDto> Options
);

// --- Product DTOs ---
public record CreateProductDto(
    string Sku,
    string Name,
    string? Description,
    decimal BasePrice,
    string CurrencyCode,
    List<Guid> ModifierGroupIds // Ürüne bağlanacak modifikatör gruplarının ID'leri
);

public record ProductDto(
    Guid Id,
    string Sku,
    string Name,
    string? Description,
    decimal BasePrice,
    string CurrencyCode,
    bool IsActive,
    List<ModifierGroupDto> ModifierGroups
);