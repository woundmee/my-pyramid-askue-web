using MyPyramidWeb.Abstractions;
using ClosedXML.Excel;
using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Services;

public class ParseExcelService : IParseExcelService
{
    // public List<string> GetTuChannelForminDevices(string org, string pathToExcelFiles, string[] tu)
    // {
    //     string path = @$"{pathToExcelFiles}/{org.ToLower()}.xlsx";
    //     var points = new List<string>();
    //
    //     for (int i = 0; i < tu.Length; i++)
    //         points.Add(PointData(path, tu[i]));
    //     
    //     return points;
    // }


    public List<CommercialData> GetTuChannelForminDevices(string org, string pathToExcelFile, string[] tu)
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

    // private string PointData(string excelFile, string tu)
    // {
    //     if (File.Exists(excelFile))
    //     {
    //         using var workbook = new XLWorkbook(excelFile);
    //         var worksheet = workbook.Worksheet("Лист1");
    //
    //         int rowCount = worksheet.Rows().Count() + 1; // max rows
    //
    //         for (int i = 6; i <= rowCount; i++)
    //         {
    //             string cellNamePoint = worksheet.Cell(i, 2).Value.ToString();
    //             string cellMainDeviceName = GetCellValue(worksheet.Cell(i, 8));
    //             string cellMainDeviceIp = GetCellValue(worksheet.Cell(i, 9));
    //             string cellReserveDeviceName = GetCellValue(worksheet.Cell(i, 13));
    //             string cellReserveDeviceIp = GetCellValue(worksheet.Cell(i, 14));
    //             
    //             if (tu.Contains("ТУ")) 
    //                 tu = tu.Replace("ТУ", "");
    //             
    //             if (cellNamePoint.Contains("ТУ" + tu))
    //             {
    //                 string result = $"{cellNamePoint}," +
    //                                 $"{cellMainDeviceName}," +
    //                                 $"{cellMainDeviceIp}," +
    //                                 $"{cellReserveDeviceName}," +
    //                                 $"{cellReserveDeviceIp}";
    //
    //                 return result;
    //             }
    //         }
    //         return "ТУ не найдена...";
    //     }
    //     return "Файл не существует или задан неправильный путь.";
    // }
}