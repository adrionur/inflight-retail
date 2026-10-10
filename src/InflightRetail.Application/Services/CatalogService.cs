using InflightRetail.Application.Common.Interfaces;
using InflightRetail.Application.DTOs;
using InflightRetail.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InflightRetail.Application.Services;

public class CatalogService : ICatalogService
{
    private readonly IAppDbContext _context;

    public CatalogService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ModifierGroupDto> CreateModifierGroupAsync(CreateModifierGroupDto dto, CancellationToken ct = default)
    {
        var group = new ModifierGroup(dto.Name, dto.MinSelection, dto.MaxSelection, dto.Description);

        foreach (var opt in dto.Options)
        {
            group.AddOption(new ModifierOption(opt.Name, opt.PriceDelta, opt.SkuCode, opt.DisplayOrder));
        }

        _context.ModifierGroups.Add(group);
        await _context.SaveChangesAsync(ct);

        return new ModifierGroupDto(
            group.Id,
            group.Name,
            group.Description,
            group.MinSelection,
            group.MaxSelection,
            group.IsActive,
            group.Options.Select(o => new ModifierOptionDto(o.Id, o.Name, o.PriceDelta, o.SkuCode, o.DisplayOrder, o.IsActive)).ToList()
        );
    }

    public async Task<List<ModifierGroupDto>> GetModifierGroupsAsync(CancellationToken ct = default)
    {
        return await _context.ModifierGroups
            .AsNoTracking()
            .Include(g => g.Options)
            .Select(g => new ModifierGroupDto(
                g.Id,
                g.Name,
                g.Description,
                g.MinSelection,
                g.MaxSelection,
                g.IsActive,
                g.Options.OrderBy(o => o.DisplayOrder).Select(o => new ModifierOptionDto(o.Id, o.Name, o.PriceDelta, o.SkuCode, o.DisplayOrder, o.IsActive)).ToList()
            ))
            .ToListAsync(ct);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto, CancellationToken ct = default)
    {
        var product = new Product(dto.Sku, dto.Name, dto.BasePrice, dto.CurrencyCode);

        // Modifikatör gruplarını bağla
        int order = 0;
        foreach (var groupId in dto.ModifierGroupIds)
        {
            product.ProductModifierGroups.Add(new ProductModifierGroup
            {
                ProductId = product.Id,
                ModifierGroupId = groupId,
                DisplayOrder = order++
            });
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync(ct);

        // Oluşturulan ürünü ilişkileriyle birlikte getir
        var created = await _context.Products
            .AsNoTracking()
            .Include(p => p.ProductModifierGroups)
                .ThenInclude(pmg => pmg.ModifierGroup)
                    .ThenInclude(mg => mg.Options)
            .FirstAsync(p => p.Id == product.Id, ct);

        return MapToProductDto(created);
    }

    public async Task<List<ProductDto>> GetProductsAsync(CancellationToken ct = default)
    {
        var products = await _context.Products
            .AsNoTracking()
            .Include(p => p.ProductModifierGroups)
                .ThenInclude(pmg => pmg.ModifierGroup)
                    .ThenInclude(mg => mg.Options)
            .ToListAsync(ct);

        return products.Select(MapToProductDto).ToList();
    }

    private static ProductDto MapToProductDto(Product p)
    {
        var modifierGroups = p.ProductModifierGroups
            .OrderBy(pmg => pmg.DisplayOrder)
            .Select(pmg => new ModifierGroupDto(
                pmg.ModifierGroup.Id,
                pmg.ModifierGroup.Name,
                pmg.ModifierGroup.Description,
                pmg.ModifierGroup.MinSelection,
                pmg.ModifierGroup.MaxSelection,
                pmg.ModifierGroup.IsActive,
                pmg.ModifierGroup.Options.OrderBy(o => o.DisplayOrder).Select(o => new ModifierOptionDto(o.Id, o.Name, o.PriceDelta, o.SkuCode, o.DisplayOrder, o.IsActive)).ToList()
            ))
            .ToList();

        return new ProductDto(
            p.Id,
            p.Sku,
            p.Name,
            p.Description,
            p.BasePrice,
            p.CurrencyCode,
            p.IsActive,
            modifierGroups
        );
    }
}