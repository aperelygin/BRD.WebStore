using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Persistence;

namespace ProductService.API.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        db.Database.Migrate(); // применит все миграции

        return host;
    }
}