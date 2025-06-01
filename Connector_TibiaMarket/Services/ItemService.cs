using Connector_TibiaMarket.Clients;
using Connector_TibiaMarket.Models;

namespace Connector_TibiaMarket.Services
{
    public class ItemService(ITibiaMarketClient client)
    {
        public async Task<ItemMetaData?> GetAsync(int id)
        {
            return await client.GetAsync<ItemMetaData>(id.ToString());
        }

        public async Task<IEnumerable<ItemMetaData>?> ListAsync()
        {
            return await client.ListAsync<ItemMetaData>("item_metadata?item_id=-1");
        }
    }
}
