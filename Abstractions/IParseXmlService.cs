using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Abstractions;

public interface IParseXmlService : IParseService
{
    int RequestIdParse(string xml);
    TuPassportData EntitiesDataParse(string xml);
}