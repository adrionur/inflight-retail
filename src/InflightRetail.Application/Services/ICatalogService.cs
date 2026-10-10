using InflightRetail.Application.DTOs;

namespace InflightRetail.Application.Services;

public interface ICatalogService
{
    Task<ModifierGroupDto> CreateModifierGroupAsync(CreateModifierGroupDto dto, CancellationToken ct = default);
    Task<List<ModifierGroupDto>> GetModifierGroupsAsync(CancellationToken ct = default);
    
    Task<ProductDto> CreateProductAsync(CreateProductDto dto, CancellationToken ct = default);
    Task<List<ProductDto>> GetProductsAsync(CancellationToken ct = default);
}