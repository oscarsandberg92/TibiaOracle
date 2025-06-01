using System.Text.Json.Serialization;

namespace Connector_TibiaMarket.Models
{
    public class MarketData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("time")]
        public double Time { get; set; }

        [JsonPropertyName("is_full_data")]
        public bool IsFullData { get; set; }

        [JsonPropertyName("buy_offer")]
        public int BuyOffer { get; set; }

        [JsonPropertyName("sell_offer")]
        public int SellOffer { get; set; }

        [JsonPropertyName("month_average_sell")]
        public int MonthAverageSell { get; set; }

        [JsonPropertyName("month_average_buy")]
        public int MonthAverageBuy { get; set; }

        [JsonPropertyName("month_sold")]
        public int MonthSold { get; set; }

        [JsonPropertyName("month_bought")]
        public int MonthBought { get; set; }

        [JsonPropertyName("active_traders")]
        public int ActiveTraders { get; set; }

        [JsonPropertyName("month_highest_sell")]
        public int MonthHighestSell { get; set; }

        [JsonPropertyName("month_lowest_buy")]
        public int MonthLowestBuy { get; set; }

        [JsonPropertyName("month_lowest_sell")]
        public int MonthLowestSell { get; set; }

        [JsonPropertyName("month_highest_buy")]
        public int MonthHighestBuy { get; set; }

        [JsonPropertyName("buy_offers")]
        public int BuyOffers { get; set; }

        [JsonPropertyName("sell_offers")]
        public int SellOffers { get; set; }

        [JsonPropertyName("day_average_sell")]
        public int DayAverageSell { get; set; }

        [JsonPropertyName("day_average_buy")]
        public int DayAverageBuy { get; set; }

        [JsonPropertyName("day_sold")]
        public int DaySold { get; set; }

        [JsonPropertyName("day_bought")]
        public int DayBought { get; set; }

        [JsonPropertyName("day_highest_sell")]
        public int DayHighestSell { get; set; }

        [JsonPropertyName("day_lowest_sell")]
        public int DayLowestSell { get; set; }

        [JsonPropertyName("day_highest_buy")]
        public int DayHighestBuy { get; set; }

        [JsonPropertyName("day_lowest_buy")]
        public int DayLowestBuy { get; set; }

        [JsonPropertyName("total_immediate_profit")]
        public int TotalImmediateProfit { get; set; }

        [JsonPropertyName("total_immediate_profit_info")]
        public string TotalImmediateProfitInfo { get; set; }
    }

}
