
# Журнал обновлений

- 🚀 **v1.42.17**: _Список обновлений_
  - Добавлен новый сервис `IFileOperationService`, который будет заниматься операциями над файлами (копирование, перемещение и т.д.).
  - `HomeController` переименован в `PyramidController`.
  - Обновлен `PyramidController`
    - Добавлен метод `AboutMore()`, показывающий подробную информацию о проекте.
    - Добавлен метод `Users()`, который выводит список пользователей из системы ПО "Пирамида 2.0".
    - Добавлен метод `UpdateReports()`, который обновляет список отчетов, необходимых для работы приложения.
  - Добавлены новые модели хранения данных `/Models/Data/`:
    - `AppInfoData`
    - `PyramidUserData`
  - Мелкие изменения.

- 🚀 **v1.30.12**: _Список обновлений_
  - Каталог `Interfaces` переименовал в `Abstractions`.
  - Добавлены новые интерфейсы:
    - `IConfigService` - для работы с конфиг-файлом.
    - `IHttpService` - для отправления http-запросов к API ПО "Пирамида 2.0".
    - `IParseXmlService` - парсинг XML-документов (полученные от API).
    - `IPyramidApiService` - для отправления и получения необходимой информации от API.
    - `IXmlQueryService` - xml-запросы для API.
  - Реализован метод `Controllers/PointController/Passport`
  - Добавлен новый метод `Controllers/PointController/PassportPost`
  - Каталог `Models/Dto` переименован в `Models/Data`
  - Созданы новые data-классы:
    - `CommercialData`
    - `PyramidCredentialData`
    - `SendRequestParametersData`
    - `SoapActionData`
    - `TuPassportData`
  - Добавлено новое view-представление
    - `/Models/Views/TuPassportView`
  - Добавленые новые сервисы, реализующие вышеприведенные интерфейсы.

