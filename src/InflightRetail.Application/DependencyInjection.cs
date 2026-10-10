using InflightRetail.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InflightRetail.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<IFlightService, FlightService>(); // Eklendi
        return services;
    }
}