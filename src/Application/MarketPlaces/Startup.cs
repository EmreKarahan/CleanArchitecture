using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minima.MarketPlace.NOnbir;
using Minima.Trendyol.Client;
using Minima.Trendyol.Client.Constants;

namespace Application.MarketPlaces;

public static class Startup
{
    public static IServiceCollection AddMarketPlaces(this IServiceCollection services,  IConfiguration configuration)
    {
        var settings = configuration.GetSection("TrendyolSettings").Get<TrendyolSettings>();
        services.AddTrendyolClient(settings);

        NOnbirSettings? nOnbirSettings = configuration.GetSection("NOnbirSetting").Get<NOnbirSettings>();
        services.AddNOnbirClient(nOnbirSettings);

        return services;
    }
}