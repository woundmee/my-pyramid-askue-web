using MyPyramidWeb.Interfaces;
using MyPyramidWeb.Services;

namespace MyPyramidWeb.Extensions;

public static class ServicesRegistrationExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        
        services.AddScoped<IParseExcelService, ParseExcelService>();
        services.AddScoped<ParseExcelService>();
        
        return services;
    }
}