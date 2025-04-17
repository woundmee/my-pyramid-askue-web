using System.Net.Http.Headers;
using System.Text;
using MyPyramidWeb.Abstractions;
using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Services;

public class HttpService : IHttpService
{
    public async Task<HttpResponseMessage> GetResponse(HttpClient httpClient, SendRequestParametersData parameter)
    {
        // очищаю, т.к. заголовки задаю дефолтовые, которые сохраняются между запросами.
        httpClient.DefaultRequestHeaders.Clear();
        
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
        httpClient.DefaultRequestHeaders.Add("SOAPAction", parameter.SoapAction);

        var byteArray = Encoding.ASCII.GetBytes($"{parameter.Username}:{parameter.Password}");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(byteArray)
        );

        var httpContent = new StringContent(parameter.Content, Encoding.UTF8, "text/xml");
        var response = await httpClient.PostAsync(parameter.Url, httpContent);

        // Console.WriteLine(">> response: " + response);
        // Console.WriteLine(">> content: " + parameter.Content);

        if (response.IsSuccessStatusCode)
            return response;

        // if error
        Console.WriteLine($"-> ERROR: {(int)response.StatusCode}:{response.StatusCode}");
        return response;

        // return response;
    }
}