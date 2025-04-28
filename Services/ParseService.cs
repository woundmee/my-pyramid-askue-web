using System.Xml.Linq;
using ClosedXML.Excel;
using MyPyramidWeb.Abstractions;
using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Services;

public class ParseService : IParseService
{
    private readonly XNamespace _ns = "http://www.sicon.ru/Integration/Pyramid/2019/08";

    private readonly IConfigService _configService;

    public ParseService(IConfigService configService)
    {
        _configService = configService;
    }

    // Excel //////////////////////////////
    public List<CommercialData> GetNetworkDevices(string org, string pathToExcelFile, string[] tu)
    {
        string path = @$"{pathToExcelFile}/{org.ToLower()}.xlsx";
        var points = new List<CommercialData>();

        for (int i = 0; i < tu.Length; i++)
        {
            var pointData = PointData(path, tu[i]);
            // points.Add(pointData);
            foreach (var point in pointData)
            {
                points.Add(point);
            }
        }

        return points;
    }

    public List<PyramidUserData> GetPyramidUsers()
    {
        var pyramidUsers = new List<PyramidUserData>();

        foreach (var org in _configService.GetOrgs())
        {
            var users = ParceExcelDocWithPyramidUsers(
                @$"Sources/Reports/PyramidUsers/{org.ToLower()}.xlsx", org);
            foreach (var user in users)
            {
                pyramidUsers.Add(user);
            }
        }

        return pyramidUsers;
    }

    private List<PyramidUserData> ParceExcelDocWithPyramidUsers(string excelFile, string orgName)
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

    private string GetCellValue(IXLCell cell)
        => cell.Value.ToString() != string.Empty ? cell.Value.ToString() : "-";


    private List<CommercialData> PointData(string excelFile, string tu)
    {
        var pointDataList = new List<CommercialData>();
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


                if (pointData.CellNamePoint.Contains("ТУ" + tu) || pointData.CellNamePoint.Contains("ТУ-" + tu))
                    pointDataList.Add(pointData);
            }

            if (pointDataList.Count == 0)
                throw new NullReferenceException("ТУ не найдена!");

            return pointDataList;

            // message.Message = "ТУ не найдена.";
            // return message;
        }

        throw new FileNotFoundException("Файл не существует или задан неправильный путь.");

        // message.Message = "Файл не существует или задан неправильный путь.";
        // return message;
    }

    // Xml ////////////////////////////////
    public int GetRequestId(string xml)
    {
        XDocument xdoc = XDocument.Parse(xml);
        var requestId = xdoc.Descendants(_ns! + "RequestId").FirstOrDefault();

        return int.Parse(requestId?.Value!);
        // catch
        // {
        //     throw new Exception("RequestID не получен!");
        // }
    }

    public TuPassportData GetTuPassport(string xml)
    {
        XDocument xdoc = XDocument.Parse(xml);


        string tekonReplacementComment =
            GetXmlValueThreeLevel(xdoc, "Meters", "PipePointMeter", "ReplacementReason") ?? "null";
        string tekonReplacement = GetXmlValueThreeLevel(xdoc, "Meters", "PipePointMeter", "UninstallDate") ?? "null";

        var tekonPassportData = new TuPassportData()
        {
            TuId = GetXmlValueTwoLevel(xdoc, "PipePoint", "Id") ?? "null",
            TuNumber = GetXmlValueTwoLevel(xdoc, "PipePoint", "Name") ?? "null",
            TekonSerialNumber = GetXmlValueTwoLevel(xdoc, "Meter", "SerialNumber") ?? "null",
            TekonTypeResourse = GetXmlValueTwoLevel(xdoc, "Meter", "Model") ?? "null",
            TekonInstallDate = GetXmlValueThreeLevel(xdoc, "Meters", "PipePointMeter", "InstallDate") ?? "null",
            TekonReplacement = tekonReplacement == "" ? "..." : tekonReplacement,
            TekonReplacementComment = tekonReplacementComment == "" ? "..." : tekonReplacementComment
        };

        return tekonPassportData;

        // catch (ArgumentNullException)
        // {
        //     throw new ArgumentNullException("Не удалось обработать паспорт ТУ! Пустой вывод...");
        // }
    }


    private string? GetXmlValueOneLevel(XDocument xdoc, string level1)
        => xdoc.Descendants(_ns! + level1).FirstOrDefault()?.Value;

    // можно добавить: int skipCount = 0
    private string? GetXmlValueTwoLevel(XDocument xdoc, string level1, string level2)
        => xdoc.Descendants(_ns! + level1).Select(x => x.Element(_ns! + level2))?.FirstOrDefault()?.Value;


    private string? GetXmlValueThreeLevel(XDocument xdoc, string level1, string level2, string level3, int skip = 0)
        => xdoc.Descendants(_ns! + level1)
            .Select(x => x.Element(_ns! + level2)).Skip(skip)
            .Select(x => x?.Element(_ns! + level3))
            // .Select(x => x.Element(ns! + level3))?
            .LastOrDefault()?.Value;
    // fixme: выбран LastOrDefault()
}