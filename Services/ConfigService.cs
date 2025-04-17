using Microsoft.Extensions.Options;
using MyPyramidWeb.Abstractions;
using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Services;

public class ConfigService : IConfigService
{
    // private readonly IConfiguration _config;

    private readonly Dictionary<string, Dictionary<string, PyramidCredentialData>> _pyramidCredentials;
    private readonly Dictionary<string, SoapActionData> _soapAction;

    public ConfigService(
        IOptions<Dictionary<string, Dictionary<string, PyramidCredentialData>>> pyramidCredentials,
        IOptions<Dictionary<string, SoapActionData>> soapAction)
    {
        _pyramidCredentials = pyramidCredentials.Value;
        _soapAction = soapAction.Value;
    }

    public Dictionary<string, Dictionary<string, PyramidCredentialData>> GetPyramidCredentials()
    {
        return _pyramidCredentials;
    }

    public Dictionary<string, SoapActionData> GetSoapActions()
    {
        return _soapAction;
    }
}