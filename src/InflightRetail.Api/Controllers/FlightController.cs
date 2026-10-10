using InflightRetail.Application.DTOs;
using InflightRetail.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InflightRetail.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightController : ControllerBase
{
    private readonly IFlightService _flightService;

    public FlightController(IFlightService flightService)
    {
        _flightService = flightService;
    }

    [HttpPost]
    public async Task<ActionResult<FlightSummaryDto>> CreateFlight([FromBody] CreateFlightDto dto, CancellationToken ct)
    {
        var result = await _flightService.CreateFlightAsync(dto, ct);
        return CreatedAtAction(nameof(GetFlights), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<List<FlightSummaryDto>>> GetFlights(CancellationToken ct)
    {
        var result = await _flightService.GetFlightsAsync(ct);
        return Ok(result);
    }

    /// <summary>
    /// Kabin tabletinin uçuş öncesi indireceği tek parça optimize katalog ve stok manifestosu
    /// </summary>
    [HttpGet("{id:guid}/manifest")]
    public async Task<ActionResult<FlightManifestDto>> GetFlightManifest(Guid id, CancellationToken ct)
    {
        var manifest = await _flightService.GetFlightManifestAsync(id, ct);
        if (manifest == null)
            return NotFound(new { Message = "Uçuş bulunamadı." });

        return Ok(manifest);
    }
}