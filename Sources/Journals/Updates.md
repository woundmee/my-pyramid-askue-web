
# Журнал обновлений
Здесь будут фиксироваться обновления приложения.
<br>

- **v1.30.12**: Список обновлений
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

