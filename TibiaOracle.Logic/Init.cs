using Connector_TibiaData;
using Microsoft.Extensions.DependencyInjection;
using TibiaOracle.Logic.Services;

namespace TibiaOracle.Logic
{
    public static class Init
    {
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            services.AddTibiaDataConnector();
            services.AddScoped<HouseLogic>();

            return services;
        }
    }
}
