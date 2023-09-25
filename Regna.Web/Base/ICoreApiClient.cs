namespace Regna.Web.Base
{
    public interface ICoreApiClient
    {
        Task<HttpResponseMessage> ApiRequest<T>(string controllerName, string actionName, T data);
    }
}
