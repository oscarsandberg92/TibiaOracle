using Connector_TibiaData.Models;
using Microsoft.AspNetCore.Mvc;
using TibiaOracle.Logic.Services;

namespace TibiaOracle.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HouseController(HouseLogic houseLogic, ILogger<HouseController> _logger) : ControllerBase
    {
        [HttpGet(Name = "GetAvailableHouses")]
        public async Task<ActionResult<HouseDetails>> GetAvailableHouses([FromQuery]string world)
        {
            var houses = await houseLogic.GetAllAuctionedHouses(world);

            return Ok(houses);
        }
    }
}
