using DocumentFormat.OpenXml.Wordprocessing;
using MyPyramidWeb.Abstractions;

namespace MyPyramidWeb.Services;

public class FileOperationsService : IFileOperationsService
{
    private readonly IConfiguration _configuration;
    private readonly IConfigService _configService;

    public FileOperationsService(IConfiguration configuration, IConfigService configService)
    {
        _configuration = configuration;
        _configService = configService;
    }

    public void CopyExcelReportsToProject(string configNodeName, string destinationFolder, string filename,
        string fileExtension)
    {
        // string zabbixSharePath = _configuration.GetValue<string>("App:ZabbixShare")!;
        string zabbixSharePath = _configuration.GetValue<string>(configNodeName)!;
        foreach (var org in _configService.GetOrgs())
        {
            string sourceFile = Path.GetFullPath(zabbixSharePath + org + $"\\{filename}.{fileExtension}");
            string destFile = Path.GetFullPath(@$"Sources\Reports\{destinationFolder}\{org.ToLower()}.{fileExtension}");

            try
            {
                File.Copy(sourceFile, destFile, true);
            }
            catch (FileNotFoundException fileNotFoundEx)
            {
                continue; 
                // потому что отчет содержится не во всех каталогах
                
                throw new FileNotFoundException($"❌ ERROR: Файл {sourceFile} отсутствует");
            }
        }
    }
}