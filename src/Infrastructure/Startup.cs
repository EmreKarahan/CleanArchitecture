using Infrastructure.Caching;
using Infrastructure.Files;
using Infrastructure.Quartz;
using Infrastructure.Services;

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


    public static IApplicationBuilder UseInfrastructureServices(this IApplicationBuilder app)
    {
        app.UseCaching();
        return app;
    }
}