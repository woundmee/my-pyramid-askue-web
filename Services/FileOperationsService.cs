using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using MyPyramidWeb.Abstractions;

namespace MyPyramidWeb.Services;

public class FileOperationsService : IFileOperationsService
{
    private readonly IConfiguration _configuration;
    private readonly IConfigService _configService;
    private readonly IParseService _parseService;

    public FileOperationsService(
        IConfiguration configuration,
        IConfigService configService,
        IParseService parseService
    )
    {
        _configuration = configuration;
        _configService = configService;
        _parseService = parseService;
    }

    public void CopyExcelReportsToProject(string configNodeName, string destinationFolder, string filename,
        string fileExtension)
    {
        // string zabbixSharePath = _configuration.GetValue<string>("App:ZabbixShare")!;
        string zabbixSharePath = _configuration.GetValue<string>(configNodeName)!;
        foreach (var org in _configService.GetOrgs())
        {
            string sourceFile = Path.GetFullPath(zabbixSharePath + org + $"/{filename}.{fileExtension}");
            string destFile = Path.GetFullPath($"Sources/Reports/{destinationFolder}/{org.ToLower()}.{fileExtension}");

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

    public void SaveExcelDocumentWithUsers(MemoryStream stream)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Пользователи АСКУЭ");

        // cell setttings
        worksheet.Range("A1:F1").Style.Fill.BackgroundColor = XLColor.FromArgb(1, 255, 232, 189);
        worksheet.Range("A1:F1").Style.Font.FontSize = 12;
        worksheet.Range("A1:F1").Style.Font.Bold = true;
        worksheet.Range("A1:F1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Range("A1:F1").Worksheet.ColumnWidth = 40;

        worksheet.Cell(1, 1).Value = "Организация";
        worksheet.Cell(1, 2).Value = "Логин";
        worksheet.Cell(1, 3).Value = "Фамилия";
        worksheet.Cell(1, 4).Value = "Имя";
        worksheet.Cell(1, 5).Value = "Отчетство";
        worksheet.Cell(1, 6).Value = "Email";

        var users = _parseService.GetPyramidUsers();
        for (int i = 2; i <= users.Count; i++)
        {
            worksheet.Cell(i, 1).Value = users[i - 1].Organization;
            worksheet.Cell(i, 2).Value = users[i - 1].Login;
            worksheet.Cell(i, 3).Value = users[i - 1].SurName;
            worksheet.Cell(i, 4).Value = users[i - 1].Name;
            worksheet.Cell(i, 5).Value = users[i - 1].Patronymic;
            worksheet.Cell(i, 6).Value = users[i - 1].Email;
        }

        worksheet.Cells().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
        // workbook.SaveAs("Pyramid users.xlsx");
        workbook.SaveAs(stream);
        stream.Position = 0;
    }
}