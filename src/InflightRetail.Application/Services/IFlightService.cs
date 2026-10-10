using InflightRetail.Application.DTOs;

namespace InflightRetail.Application.Services;

public interface IFlightService
{
    Task<FlightSummaryDto> CreateFlightAsync(CreateFlightDto dto, CancellationToken ct = default);
    Task<List<FlightSummaryDto>> GetFlightsAsync(CancellationToken ct = default);
    Task<FlightManifestDto?> GetFlightManifestAsync(Guid flightId, CancellationToken ct = default);
}