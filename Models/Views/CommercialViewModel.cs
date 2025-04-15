using MyPyramidWeb.Models.Dto;

namespace MyPyramidWeb.Models.Views;

public class CommercialViewModel
{
    public required List<CommercialDataDtoModel> PointData { get; set; }
    public string[] Orgs { get; set; } = [];
}