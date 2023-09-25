using Newtonsoft.Json;

namespace Regna.Web.Base
{
    public class CoreApiClient : ICoreApiClient
    {
        private string ServiceUrl { get; }

        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        public CoreApiClient( IConfiguration config)
        {
            _httpClient = new HttpClient();
            _config = config;
            ServiceUrl = _config.GetValue<string>("CoreApiAddress");
        }

        public async Task<HttpResponseMessage> ApiRequest<T>(string controllerName, string actionName, T data)
        {
            var url = ServiceUrl + "/" + controllerName + "/" + actionName;
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, data);
            return response;
        }
    }
}
