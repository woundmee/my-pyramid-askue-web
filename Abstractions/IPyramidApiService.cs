using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Abstractions;

public interface IPyramidApiService
{
    Task<int> GetRequestId(HttpClient httpClient, SendRequestParametersData requestParameters);
    Task<TuPassportData[]> GetPassportTuArray(string[] tu);
}