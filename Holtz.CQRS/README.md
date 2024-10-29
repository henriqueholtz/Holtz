# Holtz.CQRS

This is an web api built with `CQRS (Command and Query Responsibility Segregation)` which is a pattern that separates read and update operations. But it also includes:

- :heavy_check_mark: **.NET 8**
- :heavy_check_mark: **MediatR**
- :heavy_check_mark: **Code First**
- :heavy_check_mark: **Entity Framework Core**
- :heavy_check_mark: **SQLite**
- :heavy_check_mark: **Unit tests using Moq**
- :heavy_check_mark: **Integration tests**
- :heavy_check_mark: **Arch tests**
- :heavy_check_mark: **Middleware**
- :heavy_check_mark: **FluentValidation**
- :heavy_check_mark: **Docker support**
- :heavy_check_mark: **.NET Aspire**

## Requirements

- .NET SDK 8+

## How to run

- **Using Visual Studio (2022+):** Just select the type like `IIS Express` or `Docker` and hit the play button
- **dotnet cli**: `dotnet run --project ./Holtz.CQRS.Api --urls="https://localhost:32768" --watch`

## How to test

```
dotnet test
```

### Migrations

Generate: `dotnet ef migrations add message -o Migrations` (from `Holtz.CQRS.Infraestructure` folder)
Apply: `dotnet ef database update` (from `Holtz.CQRS.Api` folder)

### TO DO list

- Add Unit tests
- Add integration tests
- Add support to Windows container
- Add swagger annotations
- Add notification
- Add FluentValidation (include swagger)
- Add Mappers
