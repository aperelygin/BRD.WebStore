using ProductService.API.Extensions;
using ProductService.API.Services;
using ProductService.Infrastructure.Bootstrap;

namespace ProductService.API;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args)
            .Build()
            .MigrateDatabase()  // добавили миграцию красиво
            .Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                // можно накинуть .env, secrets.json, etc.
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices((context, services) =>
                {
                    services.AddGrpc();

                    services.AddInfrastructure(context.Configuration);
                });

                webBuilder.Configure(app =>
                {
                    var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();

                    if (env.IsDevelopment())
                    {
                        // какие-нибудь middleware, если нужны
                    }

                    app.UseRouting();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGrpcService<ProductGrpcService>();
                        endpoints.MapGet("/", () => "ProductService gRPC is running");
                    });
                });
            });
}