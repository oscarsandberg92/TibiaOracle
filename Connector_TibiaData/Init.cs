using Connector_TibiaData.Clients;
using Connector_TibiaData.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Connector_TibiaData
{
    public static class Init
    {
        public static IServiceCollection AddTibiaDataConnector(this IServiceCollection services)
        {
            services.AddHttpClient("TibiaApi", client =>
            {
                client.BaseAddress = new Uri("https://api.tibiadata.com/v4/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });


            services.AddScoped<ITibiaDataClient, TibiaDataClient>();

            services.AddScoped<HouseService>();

            return services;
        }
    }
}
