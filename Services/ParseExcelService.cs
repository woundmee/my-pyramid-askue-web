using MyPyramidWeb.Abstractions;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Services;

public class ParseExcelService : IParseExcelService
{
    private readonly IConfigService _configService;

    public ParseExcelService(IConfigService configService)
        => _configService = configService;

    public List<PyramidUserData> ParsePyramidUsers()
    {
        var pyramidUsers = new List<PyramidUserData>();

        foreach (var org in _configService.GetOrgs())
        {
            var users = ParsePyramidUser(
                @$"Sources/Reports/PyramidUsers/{org.ToLower()}.xlsx", org);
            foreach (var user in users)
            {
                pyramidUsers.Add(user);
            }
        }

        return pyramidUsers;
    }


    private List<PyramidUserData> ParsePyramidUser(string excelFile, string orgName)
    {
        if (File.Exists(excelFile))
        {
            var users = new List<PyramidUserData>();

            using var workbook = new XLWorkbook(excelFile);
            var worksheet = workbook.Worksheet("Лист1");

            // int rowCount = worksheet.Rows().Count() + 1; // max rows
            int rowCount = worksheet.RowsUsed().Count();

            try
            {
                for (int i = 2; i <= rowCount; i++)
                {
                    string patronymic = GetCellValue(worksheet.Cell(i, 5));
                    string login = GetCellValue(worksheet.Cell(i, 2));
                    string email = login + "@nornik.ru; ";

                    // 🚩 BUG: некорректно отрабатывает. Починить.
                    // if (_configService.GetExcludedMails().Contains(login))
                    //     email = "";

                    var pyramidUser = new PyramidUserData()
                    {
                        Organization = orgName,
                        Login = GetCellValue(worksheet.Cell(i, 2)),
                        Email = email, //GetCellValue(worksheet.Cell(i, 2)) + "@nornik.ru; ",
                        SurName = GetCellValue(worksheet.Cell(i, 3)),
                        Name = GetCellValue(worksheet.Cell(i, 4)),
                        Patronymic = patronymic == "" ? "-" : patronymic
                    };
                    users.Add(pyramidUser);
                }

                return users;
            }
            catch
            {
                throw new Exception("🚩 Пользователь не найден.");
            }
        }

        return new List<PyramidUserData>();
        // throw new ArgumentException("🚩 Excel-файл не найден или не существует.");
    }


    public List<CommercialData> ParseDevices(string org, string pathToExcelFile, string[] tu)
    {
        string path = @$"{pathToExcelFile}/{org.ToLower()}.xlsx";
        var points = new List<CommercialData>();

        for (int i = 0; i < tu.Length; i++)
        {
            var pointData = PointData(path, tu[i]);
            points.Add(pointData);
        }

        return points;
    }

    private string GetCellValue(IXLCell cell)
        => cell.Value.ToString() != string.Empty ? cell.Value.ToString() : "-";

    private CommercialData PointData(string excelFile, string tu)
    {
        var message = new CommercialData();

        if (File.Exists(excelFile))
        {
            using var workbook = new XLWorkbook(excelFile);
            var worksheet = workbook.Worksheet("Лист1");

            // int rowCount = worksheet.Rows().Count() + 1; // max rows
            int rowCount = worksheet.RowsUsed().Count() + 1;

            for (int i = 6; i <= rowCount; i++)
            {
                var pointData = new CommercialData
                {
                    CellNamePoint = worksheet.Cell(i, 2).Value.ToString(),
                    CellMainDeviceName = GetCellValue(worksheet.Cell(i, 8)),
                    CellMainDeviceIp = GetCellValue(worksheet.Cell(i, 9)),
                    CellReserveDeviceName = GetCellValue(worksheet.Cell(i, 13)),
                    CellReserveDeviceIp = GetCellValue(worksheet.Cell(i, 14))
                };

                if (pointData.CellNamePoint.Contains("ТУ" + tu))
                    return pointData;
            }

            message.Message = "ТУ не найдена.";
            return message;
        }

        message.Message = "Файл не существует или задан неправильный путь.";
        return message;
    }
}