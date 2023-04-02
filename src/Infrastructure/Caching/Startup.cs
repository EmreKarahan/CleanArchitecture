using Application.Common.Caching;

namespace Infrastructure.Caching;

public static class Startup
{
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEnyimMemcached(options => configuration.GetSection("enyimMemcached").Bind(options));
        services.AddScoped<ICachingManager, MemcachedManager>();
        return services;
    }
    
    public static IApplicationBuilder UseCaching(this IApplicationBuilder app)
    { 
        app.UseEnyimMemcached();
        return app;
    }
}