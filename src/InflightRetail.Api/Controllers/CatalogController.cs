using InflightRetail.Application.DTOs;
using InflightRetail.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InflightRetail.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    // --- MODIFIER GROUPS ---

    [HttpPost("modifier-groups")]
    public async Task<ActionResult<ModifierGroupDto>> CreateModifierGroup(
        [FromBody] CreateModifierGroupDto dto,
        CancellationToken ct)
    {
        var result = await _catalogService.CreateModifierGroupAsync(dto, ct);
        return CreatedAtAction(nameof(GetModifierGroups), new { id = result.Id }, result);
    }

    [HttpGet("modifier-groups")]
    public async Task<ActionResult<List<ModifierGroupDto>>> GetModifierGroups(CancellationToken ct)
    {
        var result = await _catalogService.GetModifierGroupsAsync(ct);
        return Ok(result);
    }

    // --- PRODUCTS ---

    [HttpPost("products")]
    public async Task<ActionResult<ProductDto>> CreateProduct(
        [FromBody] CreateProductDto dto,
        CancellationToken ct)
    {
        var result = await _catalogService.CreateProductAsync(dto, ct);
        return CreatedAtAction(nameof(GetProducts), new { id = result.Id }, result);
    }

    [HttpGet("products")]
    public async Task<ActionResult<List<ProductDto>>> GetProducts(CancellationToken ct)
    {
        var result = await _catalogService.GetProductsAsync(ct);
        return Ok(result);
    }
}