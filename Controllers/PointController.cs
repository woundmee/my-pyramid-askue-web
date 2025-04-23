using Microsoft.AspNetCore.Mvc;
using MyPyramidWeb.Abstractions;
using MyPyramidWeb.Models.Data;
using MyPyramidWeb.Models.Views;


namespace MyPyramidWeb.Controllers;

public class PointController : Controller
{
    // private readonly IParseExcelService _excelService;
    private readonly IParseExcelService _excelService;
    private readonly IConfiguration _config;
    // private readonly IPyramidApiService _pyramidApiService;

    private readonly IConfigService _configService;
    private readonly IPyramidApiService _pyramidApiService;

    // private readonly IHttpService _httpService;
    // private readonly IXmlQueryService _xmlQueryService;


    public PointController(
        IParseExcelService excelService,
        IConfiguration config,
        IConfigService configService,
        IPyramidApiService pyramidApiService
        // IPyramidApiService pyramidApiService,
        // IXmlQueryService xmlQueryService,
    )
    {
        _excelService = excelService;
        _config = config;
        _configService = configService;
        _pyramidApiService = pyramidApiService;
        // _xmlQueryService = xmlQueryService;
    }

    [HttpPost]
    public async Task<IActionResult> PassportPost()
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


    [HttpGet]
    public IActionResult Passport()
    {
        var passportTuView = new TuPassportView
        {
            TuPassportDatas = new List<TuPassportData>()
        };
        
        return View(passportTuView);
    }


    [HttpPost]
    public IActionResult CommercialPost()
    {
        string orgName = Request.Form["selectOrgName"].ToString();
        string[] tuNumber = Request.Form["tuNumber"].ToString().Replace(" ", "").Split(",");


        string path = @"Sources/Reports/NetControl";

        List<CommercialData> pointData = _excelService.ParseDevices(orgName, path, tuNumber);

        var commercialView = new CommercialView()
        {
            PointData = pointData,
            Orgs = _config.GetSection("Pyramid:Orgs").GetChildren()
                .Select(x => x.Value)
                .ToArray()!
        };

        return View("Commercial", commercialView);
    }

    [HttpGet]
    public IActionResult Commercial()
    {
        var commercialView = new CommercialView()
        {
            PointData = new List<CommercialData>(),
            Orgs = _config.GetSection("Pyramid:Orgs").GetChildren()
                .Select(x => x.Value)
                .ToArray()!
        };

        return View(commercialView);
    }
}