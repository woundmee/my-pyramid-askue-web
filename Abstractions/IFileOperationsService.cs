using Microsoft.AspNetCore.Mvc;

namespace MyPyramidWeb.Abstractions;

public interface IFileOperationsService
{
    /// <param name="configNodeName">Тег, указанный в appconfig.json, который вернет путь к каталогу, где находится файл</param>
    /// <param name="filename">Имя файла</param>
    /// <param name="fileExtension">Расширение файла</param>
    /// <param name="destinationFolder">Конечный каталог, куда необходимо сохранить файл</param>
    void CopyExcelReportsToProject(string configNodeName, string destinationFolder, string filename, string fileExtension);

    void SaveExcelDocumentWithUsers(MemoryStream stream);
}