using Microsoft.AspNetCore.Mvc;

namespace MyPyramidWeb.Controllers;

public class InfrastructureController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}