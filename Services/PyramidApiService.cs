using System.Runtime.CompilerServices;
using MyPyramidWeb.Abstractions;
using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Services;

public class PyramidApiService : IPyramidApiService
{
    private readonly IConfiguration _config;
    private readonly IParseXmlService _xmlParseService;
    private readonly IHttpService _httpService;
    private readonly IXmlQueryService _xmlQueryService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfigService _configService;

    public PyramidApiService(
        IConfiguration config,
        IParseXmlService xmlParseService,
        IXmlQueryService xmlQueryService,
        IHttpService httpService,
        IHttpClientFactory httpClientFactory,
        IConfigService configService
    )
    {
        _config = config;
        _xmlParseService = xmlParseService;
        _xmlQueryService = xmlQueryService;
        _httpService = httpService;
        _httpClientFactory = httpClientFactory;
        _configService = configService;
    }

    // MAIN //////////////////////

    public async Task<TuPassportData[]> GetPassportTuArray(string[] tu)
    {
        int tuLength = tu.Length;

        // TuPassportData tuPassport = new TuPassportData();
        var tuPassportList = new List<TuPassportData>();
        
        for (int i = 0; i < tuLength; i++)
        {
            var passportTu = await GetPassportTU(tu[i]);
            var tuPassport = new TuPassportData
            {
                TuId = passportTu.TuId,
                TuNumber = passportTu.TuNumber,
                TekonSerialNumber = passportTu.TekonSerialNumber,
                TekonTypeResourse = passportTu.TekonTypeResourse,
                TekonInstallDate = passportTu.TekonInstallDate,
                TekonReplacement = passportTu.TekonReplacement,
                TekonReplacementComment = passportTu.TekonReplacementComment
            };
        
            tuPassportList.Add(tuPassport);
        }
        
        return tuPassportList.ToArray();
        

        // throw new Exception("🚩 [временно] Какая-то ошибка... Узнать и обработать :)");
    }

    public async Task<TuPassportData> GetPassportTU(string tu)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        var rokses = _configService.GetPyramidCredentials();

        foreach (var roks in rokses)
        {
            foreach (var cred in roks.Value)
            {
                Console.WriteLine($"🔸🔸🔸 ::: {cred.Value.Subject}");
                try
                {
                    var parameters = SetParametersFromRequestEntities(
                        cred.Value.Subject,
                        tu,
                        cred.Value.Url,
                        cred.Value.Username,
                        cred.Value.Password
                    );

                    var requestId = await GetRequestId(httpClient, parameters);

                    // stat step2
                    parameters.Content = _xmlQueryService.GetQueryFromEntitiesStep2(cred.Value.Subject, requestId);
                    parameters.SoapAction = GetSoapActionFromRequestEntities(x => x.Step2); // set step2

                    var entityDataXml = await _httpService.GetResponse(httpClient, parameters);

                    int entityDataXmlLength = entityDataXml.Content.ReadAsStringAsync().Result.Length;
                    if (entityDataXmlLength < 500) continue;

                    var entityData = _xmlParseService.EntitiesDataParse(
                        await entityDataXml.Content.ReadAsStringAsync());

                    return entityData;
                }
                catch (InvalidOperationException ioEx)
                {
                    throw new InvalidOperationException(
                        $"🚩 Ошибка при обработке {cred.Value.Subject}: {ioEx.Message}\n{ioEx}");
                }
                catch (AggregateException aggEx)
                {
                    foreach (var error in aggEx.InnerExceptions)
                        throw new AggregateException($"🚩 Ошибка при обработке {cred.Value.Subject}: {error.Message}");
                }
            }
        }

        throw new InvalidOperationException("🚩 Данные отсутствуют или заданы неправильно!");
    }


    public async Task<int> GetRequestId(HttpClient httpClient, SendRequestParametersData parameters)
    {
        var response = await _httpService
            .GetResponse(httpClient, parameters)
            .Result.Content.ReadAsStringAsync();

        // Console.WriteLine("\n >> resp: " + response);
        int requestId = _xmlParseService.RequestIdParse(response);
        return requestId;
    }


    private string GetSoapActionFromRequestEntities(Func<SoapActionData, string> soapActionSelector)
    {
        return _configService.GetSoapActions()
                   .Where(x => x.Key == "Entities")
                   .Select(x => soapActionSelector.Invoke(x.Value)).FirstOrDefault()
               ?? throw new ArgumentNullException("🚩 По ключу отсутствует SoapAction");


        var soapAction = _config.GetSection("SoapActionEntities").GetChildren()
            .Select(x => new SoapActionData()
            {
                Step1 = x.Value,
                Step2 = x.Value
            })
            .Select(soapActionSelector)
            .FirstOrDefault();

        Console.WriteLine($"❌❌ {soapAction}");

        return soapAction ?? throw new Exception("🚩 SoapAction is null!");

        // return _config.GetSoapActions()
        //            .Where(x => x.Key == "soapActionEntities")
        //            .Select(x => soapActionSelector.Invoke(x.Value)).FirstOrDefault()
        //        ?? throw new ArgumentNullException("По ключу отсутствует SoapAction");
    }

    private SendRequestParametersData SetParametersFromRequestEntities(
        string subject, string? tu, string url, string username, string password)
    {
        return new SendRequestParametersData()
        {
            Content = _xmlQueryService.GetQueryFromEntitiesStep1(subject, tu!.ToUpper()),
            SoapAction = GetSoapActionFromRequestEntities(x => x.Step1), // default: Step1
            Url = url,
            Username = username,
            Password = password
        };
    }
}