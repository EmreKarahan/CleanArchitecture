using Infrastructure.Caching;
using Infrastructure.Files;
using Infrastructure.Quartz;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
        
        services.AddPersistence(configuration);
        services.AddCaching(configuration);
        services.AddScheduledJob(configuration);
        
        return services;
    }


    public static IApplicationBuilder UseInfrastructureServices(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            // Initialise and seed database
            using var scope = app.ApplicationServices.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
            initializer.InitialiseAsync().GetAwaiter().GetResult();
            initializer.SeedAsync().GetAwaiter().GetResult();
        }

        app.UseCaching();
        return app;
    }
}