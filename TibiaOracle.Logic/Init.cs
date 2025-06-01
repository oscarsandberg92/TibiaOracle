using Connector_TibiaData;
using Connector_TibiaMarket;
using Microsoft.Extensions.DependencyInjection;
using TibiaOracle.Logic.Services;

namespace TibiaOracle.Logic
{
    public static class Init
    {
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            services.AddTibiaDataConnector();
            services.AddTibiaMarketConnector();

            services.AddScoped<HouseLogic>();
            services.AddScoped<MarketLogic>();

            return services;
        }
    }
}
