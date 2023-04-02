namespace Infrastructure.Persistence;

public static class Startup
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("CleanArchitectureDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
        return services;
    }
    
    
    public static IApplicationBuilder UsePersistence(this IApplicationBuilder app)
    { 
        return app;
    }
}