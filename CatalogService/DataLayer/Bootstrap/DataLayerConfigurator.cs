using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using BRD.WebStore.Catalog.Infrastructure.Interfaces;

namespace BRD.WebStore.Catalog.DataLayer.Bootstrap;

public static class DataLayerConfigurator
{
    public static void ConfigureDataLayer(this WebApplicationBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
                .AddUserSecrets<DbConnectionProfile>()
                .Build();

        builder.Services.AddDbContext<CatalogDbContext>(options => 
            options.UseNpgsql(configuration.GetSection("ConnectionStrings:DefaultConnection").Value ?? ""));

        builder.Services.AddScoped<ICatalogDbContext, CatalogDbContext>();
    }

    public static void MigrateDb(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        dbContext.Database.Migrate();
    }
}
