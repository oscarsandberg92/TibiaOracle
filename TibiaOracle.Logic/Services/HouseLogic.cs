using Connector_TibiaData.Models;
using Connector_TibiaData.Services;
using TibiaOracle.Logic.Configuration;

namespace TibiaOracle.Logic.Services
{
    public class HouseLogic(HouseService houseService)
    {
        public async Task<IEnumerable<House>> GetAllAuctionedHouses(string world)
        {
            var towns = Constants.Towns;
            List<House> result = [];

            foreach (var town in towns)
            {
                var houses = await houseService.ListAsync(world, town);
                var auctionedHouses = houses.Where(h => h.Auctioned);

                result.AddRange(auctionedHouses);
            }

            return result;
        }
    }
}
