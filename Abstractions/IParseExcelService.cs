using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Abstractions;

public interface IParseExcelService : IParseService
{
    List<CommercialData> ParseDevices(string org, string pathToExcelFile, string[] tu);
    List<PyramidUserData> ParsePyramidUsers();
}
