using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Abstractions;

public interface IHttpService
{
    Task<HttpResponseMessage> GetResponse(HttpClient httpClient, SendRequestParametersData requestParameters);
}