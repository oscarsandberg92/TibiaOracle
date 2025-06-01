using Connector_TibiaMarket.Clients;
using Connector_TibiaMarket.Models;

namespace Connector_TibiaMarket.Services
{
    public class MarketService(ITibiaMarketClient client)
    {
        public async Task<IEnumerable<MarketData>> GetMarketValuesAsync(string server)
        {
            var result = await client.ListAsync<MarketData>($"market_values?server={server}&skip=0&limit=10000");
            var coin = result.FirstOrDefault(x => x.Id == 35572);


            return result.Where(x => x.TotalImmediateProfit > 0);
        }
    }
}
