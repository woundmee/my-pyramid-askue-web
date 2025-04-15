using MyPyramidWeb.Models.Dto;

namespace MyPyramidWeb.Interfaces;

public interface IParseExcelService : IParseService
{
    List<CommercialDataDtoModel> GetTuChannelForminDevices(string org, string pathToExcelFile, string[] tu);
}
