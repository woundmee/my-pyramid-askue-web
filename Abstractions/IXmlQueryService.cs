namespace MyPyramidWeb.Abstractions;

public interface IXmlQueryService
{
    string GetQueryFromEntitiesStep1(string subject, string tu);
    string GetQueryFromEntitiesStep2(string subject, int requestId);
}