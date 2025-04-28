using System.Xml;
using MyPyramidWeb.Abstractions;
using MyPyramidWeb.Models.Data;
using MyPyramidWeb.Services;

namespace MyPyramidWeb.Extensions;

public static class ServicesRegistrationExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {   
        services.AddScoped<IParseService, ParseService>();
        services.AddScoped<IXmlQueryService, XmlQueryService>();
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<IPyramidApiService, PyramidApiService>();
        services.AddScoped<IConfigService, ConfigService>();
        services.AddScoped<IFileOperationsService, FileOperationsService>();
        
        return services;
    }
}