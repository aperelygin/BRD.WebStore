using BRD.WebStore.Catalog.API.Bootstrap;

namespace BRD.WebStore.Catalog.Service.Bootstrap;

public static class ServiceConfigurator
{
    public static void RegisterServices(this IServiceCollection services)
    {
        
        
        services.RegisterWebApi();

    }
}
