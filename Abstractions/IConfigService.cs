using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Abstractions;

public interface IConfigService
{
    Dictionary<string, Dictionary<string, PyramidCredentialData>> GetPyramidCredentials();
    Dictionary<string, SoapActionData> GetSoapActions();
    AppInfoData GetAppInfo();
    List<string> GetOrgs();
    List<string> GetExcludedMails();
}