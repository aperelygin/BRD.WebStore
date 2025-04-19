using ProductService.API.Services;
using ProductService.Infrastructure.Bootstrap;

namespace ProductService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Можно сделать миграции, сиды и т.п. здесь при старте, если нужно

            host.Run();
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
}

//namespace ProductService.API
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.
//            builder.Services.AddGrpc();

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            app.MapGrpcService<GreeterService>();
//            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

//            app.Run();
//        }
//    }
//}