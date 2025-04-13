namespace Connector_TibiaData.Clients
{
    public interface ITibiaDataClient
    {
        Task<T> GetAsync<T>(string uri);
        Task<IEnumerable<T>> ListAsync<T>(string uri);
    }
}
