using Connector_TibiaData.Clients;
using Connector_TibiaData.Models;

namespace Connector_TibiaData.Services
{
    public class HouseService(ITibiaDataClient client)
    {
        public async Task<IEnumerable<House>> ListAsync(string world, string city)
        {
            var result = await client.GetAsync<HousesResponse>($"houses/{world}/{city}");

            return result.Houses.HouseList;
        }

        public async Task<HouseDetails> GetAsync(string world, int id)
        {
            var result = await client.GetAsync<HouseResponse>($"house/{world}/{id}");

            return result.House;
        }
    }
}
