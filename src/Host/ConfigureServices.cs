using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;
using ZymLabs.NSwag.FluentValidation;

namespace Host;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();


        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllers();
        ;
        services.AddScoped<FluentValidationSchemaProcessor>(provider =>
        {
            var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
        });

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddOpenApiDocument((configure, serviceProvider) =>
        {
            var fluentValidationSchemaProcessor = serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<FluentValidationSchemaProcessor>();
            // Add the fluent validations schema processor
            configure.SchemaProcessors.Add(fluentValidationSchemaProcessor);
            configure.Title = "CleanArchitecture API";
        });

        return services;
    }
}