using MyPyramidWeb.Models.Data;

namespace MyPyramidWeb.Models.Views;

public class CommercialView
{
    public required List<CommercialData> PointData { get; set; }
    public string[] Orgs { get; set; } = [];
}