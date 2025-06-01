using Connector_TibiaMarket.Clients;
using Connector_TibiaMarket.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Connector_TibiaMarket
{
    public static class Init
    {
        public static IServiceCollection AddTibiaMarketConnector(this IServiceCollection services)
        {
            services.AddHttpClient("TibiaMarket", client =>
            {
                client.BaseAddress = new Uri("https://api.tibiamarket.top:8001/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ3ZWJzaXRlIiwiaWF0IjoxNzA2Mzc2MTM1LCJleHAiOjI0ODM5NzYxMzV9.MrRgQJyNb5rlNmdsD3oyzG3ZugVeeeF8uFNElfWUOyI");
            });

            services.AddScoped<ITibiaMarketClient, TibiaMarketClient>();

            services.AddScoped<ItemService>();
            services.AddScoped<MarketService>();

            return services;
        }
    }
}
