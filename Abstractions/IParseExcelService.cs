using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Abstractions;

public interface IParseExcelService : IParseService
{
    List<CommercialData> GetTuChannelForminDevices(string org, string pathToExcelFile, string[] tu);
}
