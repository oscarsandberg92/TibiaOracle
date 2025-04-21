using Connector_TibiaData.Models;
using Connector_TibiaData.Services;
using TibiaOracle.Logic.Configuration;

namespace TibiaOracle.Logic.Services
{
    public class HouseLogic(HouseService houseService)
    {
        public async Task<IEnumerable<HouseDetails>> GetAllAuctionedHouses(string world)
        {
            var towns = Constants.Towns;
            List<HouseDetails> result = [];

            foreach (var town in towns)
            {
                var houses = await houseService.ListAsync(world, town);
                var auctionedHouses = houses.Where(h => h.Auctioned);
                foreach(var house in auctionedHouses)
                {
                    var houseDetails = await houseService.GetAsync(world, house.HouseId);
                    result.Add(houseDetails);
                }
            }

            return result;
        }
    }
}
