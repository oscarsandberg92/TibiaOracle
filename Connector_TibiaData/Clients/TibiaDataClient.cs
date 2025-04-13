using System.Text.Json;

namespace Connector_TibiaData.Clients
{
    public class TibiaDataClient :ITibiaDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        public TibiaDataClient(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("TibiaApi");

            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var deserialized = JsonSerializer.Deserialize<T>(content, _serializerOptions)!;

            return deserialized;
        }

        public async Task<IEnumerable<T>> ListAsync<T>(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<T>>(content, _serializerOptions)!;
        }
    }
}
