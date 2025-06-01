using Connector_TibiaMarket.Services;

namespace TibiaOracle.Logic.Services
{
    public class MarketLogic
    {
        private readonly MarketService _marketService;
        private readonly ItemService _itemService;

        public MarketLogic(ItemService itemService, MarketService marketService)
        {
            _itemService = itemService;
            _marketService = marketService;
        }

        public async Task<List<string>> RunAsync()
        {

            var result = await _marketService.GetMarketValuesAsync("Antica");
            var itemsMetaData = await _itemService.ListAsync();
            List<string> results = [];
            foreach (var item in result.OrderByDescending(x => x.TotalImmediateProfit))
            {
                var itemMetaData = itemsMetaData.FirstOrDefault(h => h.Id == item.Id);

                results.Add($"{itemMetaData!.Name}, {item.TotalImmediateProfit}, {string.Join(',', itemMetaData.NpcBuy.DistinctBy(x => x.Name).Select(x => x.Name + $" buys for {x.Price}"))}");
            }

            var totalProfit = result.Sum(x => x.TotalImmediateProfit);
            return results;
        }
    }
}
