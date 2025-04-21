using System.Text.Json.Serialization;

namespace Connector_TibiaData.Models
{
    public class HousesResponse
    {
        public HouseResponse_Inner Houses { get; set; } = new();
    }
    public class HouseResponse_Inner
    {
        public string World { get; set; } = string.Empty;
        public string Town { get; set; } = string.Empty;
        [JsonPropertyName("house_list")]
        public IEnumerable<House> HouseList { get; set; } = [];
    }
}
