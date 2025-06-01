using System.Text.Json.Serialization;

namespace Connector_TibiaMarket.Models
{
    public class ItemMetaData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MyProperty { get; set; }
        [JsonPropertyName("npc_buy")]
        public List<NpcBuy> NpcBuy { get; set; } = new();
    }

    public class NpcBuy
    {
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
    }
}
