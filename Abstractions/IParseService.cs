using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Abstractions;

public interface IParseService
{
    // Excel
    List<CommercialData> GetNetworkDevices(string org, string pathToExcelFile, string[] tu);
    List<PyramidUserData> GetPyramidUsers();
    Dictionary<string, int> GetPyramidLicenses();
    
    // Xml
    int GetRequestId(string xml);
    TuPassportData GetTuPassport(string xml);
}