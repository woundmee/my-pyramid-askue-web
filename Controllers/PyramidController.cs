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
    private readonly IParseService _parseService; 

    public PyramidController(IFileOperationsService fileOperations, IParseService parseService)
    {
        _fileOperations = fileOperations;
        _parseService = parseService;
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
        // _parseService.GetPyramidUsers();
        return View();
    }

    public IActionResult Licenses()
    {
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

        _fileOperations.CopyExcelReportsToProject(
            "App:ZabbixShare",
            "PyramidLicenses",
            "Pyramid Information",
            "xlsx");
        
        return RedirectToAction("Index");
    }


    [HttpGet("Download")]
    public IActionResult SavePyramidUsersInExcel()
    {
        var stream = new MemoryStream();
        _fileOperations.SaveExcelDocumentWithUsers(stream);
        
        return File(stream, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            "Пользователи АСКУЭ.xlsx");
        
        // return RedirectToAction("Users");
    }
}