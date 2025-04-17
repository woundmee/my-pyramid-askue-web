using MyPyramidWeb.Abstractions;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Drawing.Charts;
using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Services;

public class ParseXmlService : IParseXmlService
{
    private readonly XNamespace _ns = "http://www.sicon.ru/Integration/Pyramid/2019/08";

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


    // методы интерфейса

    public int RequestIdParse(string xml)
    {
        try
        {
            XDocument xdoc = XDocument.Parse(xml);
            var reqID = xdoc.Descendants(_ns! + "RequestId").FirstOrDefault();

            return int.Parse(reqID?.Value!);
        }
        catch
        {
            throw new Exception("RequestID не получен!");
        }
    }

    public TuPassportData EntitiesDataParse(string xml)
    {
        XDocument xdoc = XDocument.Parse(xml);

        try
        {
            string tekonReplacementComment = GetXmlValueThreeLevel(xdoc, "Meters", "PipePointMeter", "ReplacementReason") ?? "null";
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
        }
        catch (ArgumentNullException)
        {
            throw new ArgumentNullException("Не удалось обработать паспорт ТУ! Пустой вывод...");
        }


        // var tekonTypeResourse = GetXmlValueTwoLevel(xdoc, "Meter", "Model");
        // var tekonSerialNumber = GetXmlValueTwoLevel(xdoc, "Meter", "SerialNumber");
        // // var tuID = GetXmlValueTwoLevel(xdoc, "PipePoint", "Id");
        // var tuNumber = GetXmlValueTwoLevel(xdoc, "PipePoint", "Name");
        // var tekonInstallDate = GetXmlValueThreeLevel(xdoc, "Meters", "PipePointMeter", "InstallDate");
        // var tekonReplacement = GetXmlValueThreeLevel(xdoc, "Meters", "PipePointMeter", "UninstallDate");
        // var tekonReplacementComment =
        //     GetXmlValueThreeLevel(xdoc, "Meters", "PipePointMeter", "ReplacementReason") ?? "null";
        // // var tekonInstallLast = "...";

        // string? replacement = tekonReplacement == "" ? "..." : tekonReplacement;
        // string? replacementComment = tekonReplacementComment == "" ? "..." : tekonReplacementComment;
        //
        // string resultOut = $"Тип ресурса: {tekonTypeResourse}\n" +
        //                    $"СН тэкона: {tekonSerialNumber}\n" +
        //                    $"Наименование ТУ: {tuNumber}\n" +
        //                    $"Тэкон установлен впервые: {tekonInstallDate}\n" +
        //                    $"Тэкон заменён: {replacement} (коммент.: {replacementComment})\n";
        //
        // return resultOut ?? throw new ArgumentNullException("Не удалось обработать паспорт ТУ! Пустой вывод...");
    }
}