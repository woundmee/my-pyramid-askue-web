using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyPyramidWeb.Abstractions;
using MyPyramidWeb.Models;
using Path = DocumentFormat.OpenXml.Vml.Path;

namespace MyPyramidWeb.Controllers;

public class PyramidController : Controller
{
    // private readonly ILogger<HomeController> _logger;
    //
    // public HomeController(ILogger<HomeController> logger)
    // {
    //     _logger = logger;
    // }

    private readonly IFileOperationsService _fileOperations;
    private readonly IParseExcelService _parseExcelService;

    public PyramidController(IFileOperationsService fileOperations, IParseExcelService parseExcelService)
    {
        _fileOperations = fileOperations;
        _parseExcelService = parseExcelService;
    }


    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult AboutMore()
    {
        return View();
    }
    
    public IActionResult Users()
    {
        _parseExcelService.ParsePyramidUsers();
        return View();
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult UpdateReports() // update excel-files
    {
        _fileOperations.CopyExcelReportsToProject(
            "App:ZabbixShare",
            "NetControl",
            "Контроль связи",
            "xlsx");
        
        _fileOperations.CopyExcelReportsToProject(
            "App:ZabbixShare",
            "PyramidUsers",
            "Пользователи АСКУЭ",
            "xlsx");
        
        return RedirectToAction("Index");
    }
}