![Version](https://img.shields.io/badge/MyPyramid-v1.13.6-6d4aff?style=for-the-badge&logo=csharp&logoColor=white)
![C#](https://img.shields.io/badge/.NET8-6d4aff?style=for-the-badge&logo=csharp&logoColor=white)
![C#](https://img.shields.io/badge/C%23-6d4aff?style=for-the-badge&logo=csharp&logoColor=white)
![Rider](https://img.shields.io/badge/Rider-000000.svg?style=for-the-badge&logo=Rider&logoColor=white&color=black&labelColor=crimson)



# MyPyramid
Помощник при работе с ПО "Пирамида 2.0".  Основные возможности приложения:
- Взаимодействие с API
- Парсинг XML/Excel-документов
- Информация об основных и резервных каналах
- Информация из паспорта ТУ и т.д.

Веб-версия разработана взамен [консольной версии.](https://github.com/woundmee/my-pyramid-askue) Из преимуществ:
- Красивый дизайн
- Удобство при работе
- Расширенный функционал

## Использование:

Склонируй репозиторий
```bash
https://github.com/woundmee/my-pyramid-web-askue.git
```
Перейди в каталог с проектом
```bash
cd my-pyramid-web-askue-main/
```

Скорректировать файл `appconfig.json` (добавлен в `.gitignore`), собрать и запустить
```bash
vim appconfig.json  # configure
dotnet build
dotnet run
```