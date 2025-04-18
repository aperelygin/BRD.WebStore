using BRD.WebStore.Catalog.API.Controllers;
using BRD.WebStore.Catalog.API.Middleware;
using BRD.WebStore.Catalog.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;

namespace BRD.WebStore.Catalog.API.Bootstrap;

public static class ApiConfigurator
{
    public static void RegisterWebApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddMvc()
                .AddApplicationPart(typeof(ProductsController).Assembly);

        //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        //{
        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = Configuration["Jwt:Issuer"],
        //        ValidAudience = Configuration["Jwt:Audience"],
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        //    };
        //});

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "BRD.WebStore Catalog API",
                Version = "v1"
            });
            c.UseAllOfToExtendReferenceSchemas();
        });

        services.AddControllersWithViews();
    }

    public static void ConfigureAuth(this WebApplicationBuilder builder)
    {
        // Access configuration values
        var configuration = builder.Configuration;

        // Configure services
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "your-issuer",// configuration["Jwt:Issuer"],
                    ValidAudience = "your-audience",// configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key" /*configuration["Jwt:Key"]*/))
                };
            });

        // Register the UserContext service
        builder.Services.AddScoped<IUserContext, UserContext>();
    }

    public static void ConfigureApplicationApi(this WebApplication app)
    {
        app.UseCors("AllowAll");

        //app.UseMiddleware<ExceptionHandlerMiddleware>();
        //app.UseMiddleware<LoggingMiddleware>();

        app.UseUserContext();

        app.UseRouting();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", $"BRD.WebStore Catalog");
            c.DocExpansion(DocExpansion.None);
            c.RoutePrefix = "";
        });
    }
}
