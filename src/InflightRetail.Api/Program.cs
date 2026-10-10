using InflightRetail.Application;
using InflightRetail.Application.Common.Interfaces;
using InflightRetail.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

// Application Servisleri
builder.Services.AddApplication();

// Angular / Frontend için CORS Politikası
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Controller ve OpenAPI Desteği
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // OpenAPI şeması ve Scalar UI
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Inflight Retail API")
               .WithTheme(ScalarTheme.Moon);
    });

    // Seed Data
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.SeedAsync(dbContext);
}

app.UseHttpsRedirection();

// CORS Middleware'i routing öncesi devreye girer
app.UseCors("AllowLocalhost");

app.MapControllers();

// Sağlık kontrolü endpoint'i
app.MapGet("/api/health/db", async (AppDbContext dbContext) =>
{
    bool canConnect = await dbContext.Database.CanConnectAsync();
    return canConnect
        ? Results.Ok(new { Status = "Healthy", Database = "Connected", Timestamp = DateTime.UtcNow })
        : Results.Problem("Veritabanına bağlanılamadı.", statusCode: 503);
});

app.Run();