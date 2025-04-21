namespace Connector_TibiaData.Models
{
    public class HouseResponse
    {
        public HouseDetails House { get; set; } = new();
        public Information Information { get; set; } = new();
    }
}
