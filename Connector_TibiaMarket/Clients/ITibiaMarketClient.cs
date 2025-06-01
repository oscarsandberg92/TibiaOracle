namespace Connector_TibiaMarket.Clients
{
    public interface ITibiaMarketClient
    {
        Task<T> GetAsync<T>(string uri);
        Task<IEnumerable<T>> ListAsync<T>(string uri);
    }
}
