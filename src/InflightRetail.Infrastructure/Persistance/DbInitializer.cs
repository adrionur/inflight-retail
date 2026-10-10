using InflightRetail.Domain.Entities;
using InflightRetail.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace InflightRetail.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Eğer veritabanında zaten ürün varsa tekrar seed etme
        if (await context.Products.AnyAsync())
        {
            return;
        }

        // ==========================================
        // 1. MODİFİKATÖR GRUPLARI & SEÇENEKLERİ
        // ==========================================
        
        // Grup 1: Sandviç / Fırın Isıtma & Ekmek Tercihi (Zorunlu - 1 Seçim)
        var bakeryGroup = new ModifierGroup(
            name: "Fırın ve Ekmek Tercihi",
            minSelection: 1,
            maxSelection: 1,
            description: "Zorunlu fırınlama ve ekmek seçimi"
        );
        bakeryGroup.AddOption(new ModifierOption("Isıtılmış Kruvasan", priceDelta: 0.00m, skuCode: "SKU-BAK-CRW", displayOrder: 1));
        bakeryGroup.AddOption(new ModifierOption("Köy Ekmeği", priceDelta: 0.00m, skuCode: "SKU-BAK-BRD", displayOrder: 2));
        bakeryGroup.AddOption(new ModifierOption("Glutensiz Ekmek", priceDelta: 1.50m, skuCode: "SKU-BAK-GF", displayOrder: 3));

        // Grup 2: Sıcak İçecek Ekstraları (İsteğe Bağlı - Max 2 Seçim)
        var beverageExtrasGroup = new ModifierGroup(
            name: "Kahve & Çay Ekstraları",
            minSelection: 0,
            maxSelection: 2,
            description: "Süt ve aroma ilaveleri"
        );
        beverageExtrasGroup.AddOption(new ModifierOption("Ekstra Süt", priceDelta: 0.50m, skuCode: "SKU-MLK-01", displayOrder: 1));
        beverageExtrasGroup.AddOption(new ModifierOption("Yulaf Sütü", priceDelta: 1.00m, skuCode: "SKU-MLK-OAT", displayOrder: 2));
        beverageExtrasGroup.AddOption(new ModifierOption("Karamel Şurup", priceDelta: 1.00m, skuCode: "SKU-SYR-CRM", displayOrder: 3));

        // Grup 3: Atıştırmalık Ekleme (İsteğe Bağlı - Max 1 Seçim)
        var snackGroup = new ModifierGroup(
            name: "Menü Yanında İndirimli Atıştırmalık",
            minSelection: 0,
            maxSelection: 1,
            description: "Yemek yanına avantajlı atıştırmalık"
        );
        snackGroup.AddOption(new ModifierOption("Fıstık / Çerez (40g)", priceDelta: 2.00m, skuCode: "SKU-SNK-NUT", displayOrder: 1));
        snackGroup.AddOption(new ModifierOption("Belçika Çikolatası", priceDelta: 2.50m, skuCode: "SKU-SNK-CHO", displayOrder: 2));

        context.ModifierGroups.AddRange(bakeryGroup, beverageExtrasGroup, snackGroup);
        await context.SaveChangesAsync();

        // ==========================================
        // 2. ÜRÜNLER (PRODUCTS)
        // ==========================================

        var breakfastProduct = new Product(
            sku: "ML-BRK-001",
            name: "Gourmet Sıcak Kahvaltı Tabağı",
            basePrice: 9.50m,
            currencyCode: "EUR"
        );
        breakfastProduct.ProductModifierGroups.Add(new ProductModifierGroup
        {
            ProductId = breakfastProduct.Id,
            ModifierGroupId = bakeryGroup.Id,
            DisplayOrder = 1
        });
        breakfastProduct.ProductModifierGroups.Add(new ProductModifierGroup
        {
            ProductId = breakfastProduct.Id,
            ModifierGroupId = snackGroup.Id,
            DisplayOrder = 2
        });

        var chickenSandwich = new Product(
            sku: "ML-SND-002",
            name: "Izgara Tavuklu Focaccia Sandviç",
            basePrice: 7.50m,
            currencyCode: "EUR"
        );
        chickenSandwich.ProductModifierGroups.Add(new ProductModifierGroup
        {
            ProductId = chickenSandwich.Id,
            ModifierGroupId = snackGroup.Id,
            DisplayOrder = 1
        });

        var coffee = new Product(
            sku: "BV-HOT-001",
            name: "Filtre Kahve (Fresh Brew)",
            basePrice: 3.50m,
            currencyCode: "EUR"
        );
        coffee.ProductModifierGroups.Add(new ProductModifierGroup
        {
            ProductId = coffee.Id,
            ModifierGroupId = beverageExtrasGroup.Id,
            DisplayOrder = 1
        });

        var water = new Product(
            sku: "BV-COLD-001",
            name: "Doğal Kaynak Suyu (330ml)",
            basePrice: 2.00m,
            currencyCode: "EUR"
        );

        context.Products.AddRange(breakfastProduct, chickenSandwich, coffee, water);
        await context.SaveChangesAsync();

        // ==========================================
        // 3. ÖRNEK SEFER (FLIGHT: TK1981 IST -> LHR)
        // ==========================================

        var flight = new Flight(
            flightNumber: "TK1981",
            origin: "IST",
            destination: "LHR",
            departureTimeUtc: DateTime.UtcNow.AddHours(2),
            aircraftType: "Airbus A321-Neo"
        );

        // Uçuş Kurları
        flight.AddCurrencyRate("TRY", 38.50m);
        flight.AddCurrencyRate("USD", 1.08m);
        flight.AddCurrencyRate("GBP", 0.84m);

        // Sefer Stoğu (Catering Uplift)
        flight.AddMenuItem(breakfastProduct.Id, upliftQuantity: 40);
        flight.AddMenuItem(chickenSandwich.Id, upliftQuantity: 50);
        flight.AddMenuItem(coffee.Id, upliftQuantity: 100);
        flight.AddMenuItem(water.Id, upliftQuantity: 120);

        context.Flights.Add(flight);
        await context.SaveChangesAsync();
    }
}