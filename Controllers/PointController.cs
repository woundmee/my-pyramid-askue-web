using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration.UserSecrets;
using MyPyramidWeb.Interfaces;
using MyPyramidWeb.Models.Dto;
using MyPyramidWeb.Models.Views;
using MyPyramidWeb.Services;

namespace MyPyramidWeb.Controllers;

public class PointController : Controller
{
    // private readonly IParseExcelService _excelService;
    private readonly IParseExcelService _excelService;
    private readonly IConfiguration _config;

    public PointController(IParseExcelService excelService, IConfiguration config)
    {
        _excelService = excelService;
        _config = config;
    }


    [HttpGet]
    public IActionResult Passport()
    {
        return View();
    }


    [HttpPost]
    public IActionResult CommercialPost()
    {
        string orgName = Request.Form["selectOrgName"].ToString();
        string[] tuNumber = Request.Form["tuNumber"].ToString().Replace(" ", "").Split(",");
        

        string path = @"Sources/Reports/";
        
        List<CommercialDataDtoModel> pointData = _excelService.GetTuChannelForminDevices(orgName, path, tuNumber);

        var commercialViewModel = new CommercialViewModel()
        {
            PointData = pointData,
            Orgs = _config.GetSection("Pyramid:Orgs").GetChildren()
                .Select(x => x.Value)
                .ToArray()!
        };
        
        return View("Commercial", commercialViewModel);
    }
    
    [HttpGet]
    public IActionResult Commercial()
    {
        // var orgs = _config.GetSection("Pyramid:Orgs").GetChildren();
        //
        // ViewBag.pointData = TempData["pointData"];
        // ViewBag.orgs = orgs.Select(x => x.Value).ToArray();

        // return View();

        var commercialViewModel = new CommercialViewModel()
        {
            PointData = new List<CommercialDataDtoModel>(),
            Orgs = _config.GetSection("Pyramid:Orgs").GetChildren()
                .Select(x => x.Value)
                .ToArray()!
        };
        
        return View(commercialViewModel);
    }
}