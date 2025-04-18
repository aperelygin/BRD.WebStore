using BRD.WebStore.Catalog.API.Bootstrap;
using BRD.WebStore.Catalog.DataLayer.Bootstrap;
using BRD.WebStore.Catalog.Service.Bootstrap;

namespace BRD.WebStore.Catalog.Service;

public static class ServiceLoader
{
    public static WebApplication CreateApplication()
    {
		try
		{
            var builder = WebApplication.CreateBuilder();

            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(LogLevel.Error);
            builder.Logging.AddConsole();

            builder.Configuration.Sources.Clear();
            builder.Configuration.AddEnvironmentVariables();

            builder.Host.ConfigureServices((hostContext, services) =>
            {
                var configuration = (IConfigurationRoot)hostContext.Configuration;
                services.AddSingleton(configuration);

                services.RegisterServices();
            });
            builder.ConfigureAuth();
            builder.ConfigureDataLayer();

            var httpPort = int.Parse(Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORTS") ?? "8100");

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(httpPort);
                //options.ListenAnyIP(8101, listenOptions =>
                //{
                //    listenOptions.UseHttps();
                //});
            });

            var app = builder.Build();
            app.Services.MigrateDb();
            app.ConfigureApplicationApi();
            app.MapControllers();

            return app;
		}
		catch (Exception)
		{

			throw;
		}
    }
}
