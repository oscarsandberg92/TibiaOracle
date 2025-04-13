using System.Text.Json.Serialization;

namespace Connector_TibiaData.Models
{
    public class House
    {
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("house_id")]
        public int HouseId { get; set; }
        public bool Rented { get; set; }
        public bool Auctioned { get; set; }
        public int Size { get; set; }
        public Auction Auction { get; set; }
    }

    public class Auction
    {
        [JsonPropertyNameAttribute("current_bid")]
        public int CurrentBid { get; set; }
        [JsonPropertyNameAttribute("time_left")]
        public string TimeLeft { get; set; } = string.Empty;
        public bool Finished { get; set; }
    }
}
