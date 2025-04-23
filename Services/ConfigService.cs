using Microsoft.Extensions.Options;
using MyPyramidWeb.Abstractions;
using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Services;

public class ConfigService : IConfigService
{
    // private readonly IConfiguration _config;

    private readonly Dictionary<string, Dictionary<string, PyramidCredentialData>> _pyramidCredentials;
    private readonly Dictionary<string, SoapActionData> _soapAction;
    private readonly AppInfoData _appInfo;
    private readonly IConfiguration _configuration;

    public ConfigService(
        IOptions<Dictionary<string, Dictionary<string, PyramidCredentialData>>> pyramidCredentials,
        IOptions<Dictionary<string, SoapActionData>> soapAction,
        IOptions<AppInfoData> appInfo,
        IConfiguration configuration)
    {
        _pyramidCredentials = pyramidCredentials.Value;
        _soapAction = soapAction.Value;
        _appInfo = appInfo.Value;
        _configuration = configuration;
    }

    public Dictionary<string, Dictionary<string, PyramidCredentialData>> GetPyramidCredentials()
    {
        return _pyramidCredentials;
    }

    public Dictionary<string, SoapActionData> GetSoapActions()
    {
        return _soapAction;
    }

    public AppInfoData GetAppInfo()
    {   
        return _appInfo;
    }

    public List<string> GetOrgs()
    {
        var orgs = _configuration.GetSection("Pyramid:Orgs").Get<List<string>>();
        return orgs?.ToList()!;
    }

    public List<string> GetExcludedMails()
    {
        var mails = _configuration.GetSection("Pyramid:ExcludedMails").Get<List<string>>();
        return mails?.ToList()!;
    }
}