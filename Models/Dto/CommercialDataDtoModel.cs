using System.Reflection.Metadata.Ecma335;

namespace MyPyramidWeb.Models.Dto;

public class CommercialDataDtoModel
{
    public string CellNamePoint { get; set; } = "";
    public string CellMainDeviceName { get; set; } = "";
    public string CellMainDeviceIp { get; set; } = "";
    public string CellReserveDeviceName { get; set; } = "";
    public string CellReserveDeviceIp { get; set; } = "";
    public string Message { get; set; } = "";
}