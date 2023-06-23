# CQRS & MediatR Pattern

Command Query Responsibility Segregation & MediatR pattern using:

- .NET 7;
- CQRS;
- MediatR;
- Code First & EntityFrameworkCore;
- SQLite;
- Unit tests using Moq;
- Middleware;
- FluentValidation;

## Requirements

- .NET 7+

## How to run

- **Using Visual Studio (2022+):** Just select the type like `IIS Express` or `Docker` and hit the play button
- **dotnet cli**: `dotnet run --project ./Holtz.CQRS.Api --urls="https://localhost:32768" --watch`

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
