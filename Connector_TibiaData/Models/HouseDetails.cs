using System.Text.Json.Serialization;

namespace Connector_TibiaData.Models
{
    public class HouseDetails
    {
        [JsonPropertyName("houseid")]
        public int Id { get; set; }
        public string World { get; set; } = string.Empty;
        public string Town { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Beds { get; set; }
        public int Size { get; set; }
        public int Rent { get; set; }
        public string Img { get; set; } = string.Empty;
        public Status Status { get; set; } = new();
    }

    public class Status
    {
        [JsonPropertyName("is_auctioned")]
        public bool IsAuctioned { get; set; }
        [JsonPropertyName("is_rented")]
        public bool IsRented { get; set; }
        [JsonPropertyName("is_moving")]
        public bool IsMoving { get; set; }
        [JsonPropertyName("is_transfering")]
        public bool IsTransfering { get; set; }
        public string Original { get; set; } = string.Empty;
        public AuctionDetails Auction { get; set; } = new();
        public RentalDetails Rental { get; set; } = new();
    }

    public class AuctionDetails
    {
        [JsonPropertyName("current_bid")]
        public int CurrentBid { get; set; }
        [JsonPropertyName("current_bidder")]
        public string CurrentBidder { get; set; } = string.Empty;
        [JsonPropertyName("auction_ongoing")]
        public bool AuctionOngoing { get; set; }
        [JsonPropertyName("auction_end")]
        public DateTime AuctionEnd { get; set; }
    }

    public class RentalDetails
    {
        public string Owner { get; set; } = string.Empty;
        [JsonPropertyName("owner_sex")]
        public string OwnerSex { get; set; } = string.Empty;
        [JsonPropertyName("paid_until")]
        public string PaidUntil { get; set; } = string.Empty;
        [JsonPropertyName("moving_date")]
        public string MovingDate { get; set; } = string.Empty;
        [JsonPropertyName("transfer_reciever")]
        public string TransferReciever { get; set; } = string.Empty;
        [JsonPropertyName("transfer_price")]
        public int TransferPrice { get; set; }
        [JsonPropertyName("transfer_accept")]
        public bool TransferAccept { get; set; }
    }
    public class Information
    {
        [JsonPropertyName("tibia_urls")]
        public string[] Url { get; set; } = [];
    }
}
