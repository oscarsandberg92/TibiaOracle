using Microsoft.Extensions.DependencyInjection;
using TibiaOracle.JobScheduler.Services;

namespace TibiaOracle.JobScheduler
{
    public static class Init
    {
        public static IServiceCollection AddJobScheduler(this IServiceCollection services)
        {
            services.AddHostedService<AuctionedHousesService>();

            return services;
        }
    }
}
