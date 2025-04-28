using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using MyPyramidWeb.Abstractions;
using MyPyramidWeb.Models.Data;
using MyPyramidWeb.Models.Views;


namespace MyPyramidWeb.Controllers;

public class PointController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IParseService _parseService;
    private readonly IPyramidApiService _pyramidApiService;


    public PointController(
        IConfiguration configuration,
        IParseService parseService,
        IPyramidApiService pyramidApiService
        // IPyramidApiService pyramidApiService,
        // IXmlQueryService xmlQueryService,
    )
    {
        _configuration = configuration;
        _parseService = parseService;
        _pyramidApiService = pyramidApiService;
        // _xmlQueryService = xmlQueryService;
    }

    [HttpPost]
    public async Task<IActionResult> PassportPost()
    {
        try
        {
            string[] tuId = Request.Form["passportTuInput"].ToString().Replace(" ", "").Split(",");

            var passportPoints = await _pyramidApiService.GetPassportTuArray(tuId);

            var tuPassportData = new TuPassportView()
            {
                TuPassportDatas = passportPoints.ToList() ?? new List<TuPassportData>()
            };

            // Console.WriteLine("🚩🚩🚩 >>> " + string.Join(",", tuPassportData.TuPassportDatas.Select(x => x.TuNumber)));

            return View("Passport", tuPassportData);
        }
        catch (InvalidOperationException ioEx)
        {
            TempData["Error"] = ioEx.Message;
        }

        return RedirectToAction("Passport");
    }


    [HttpGet]
    public IActionResult Passport()
    {
        var passportTuView = new TuPassportView
        {
            TuPassportDatas = new List<TuPassportData>()
        };

        ViewBag.Error = TempData["Error"];
        return View(passportTuView);
    }


    [HttpPost]
    public IActionResult CommercialPost()
    {
        try
        {
            string orgName = Request.Form["selectOrgName"].ToString();
            string[] tuNumber = Request.Form["tuNumber"].ToString().Replace(" ", "").Split(",");

            // bool tekonPassword = Request.Form["tekonPasswordCheckbox"] == "on";

            string path = @"Sources/Reports/NetControl";
            List<CommercialData> pointData = _parseService.GetNetworkDevices(orgName, path, tuNumber);

            var commercialView = new CommercialView()
            {
                PointData = pointData,
                Orgs = _configuration.GetSection("Pyramid:Orgs").GetChildren()
                    .Select(x => x.Value)
                    .ToArray()!
            };

            return View("Commercial", commercialView);
        }
        catch (FileNotFoundException fnfEx)
        {
            TempData["Error"] = fnfEx.Message;
        }
        catch (NullReferenceException nullRefEx)
        {
            TempData["Error"] = nullRefEx.Message;
        }

        return Redirect($"Commercial");
    }

    [HttpGet]
    public IActionResult Commercial()
    {
        var commercialView = new CommercialView()
        {
            PointData = new List<CommercialData>(),
            Orgs = _configuration.GetSection("Pyramid:Orgs").GetChildren()
                .Select(x => x.Value)
                .ToArray()!
        };

        ViewBag.Error = TempData["Error"];

        return View(commercialView);
    }
}