# CQRS & MediatR Pattern

- .NET 7
- Code First & EntityFrameworkCore
- SQLite
- Unit tests using Moq;

## Requirements

- .NET 7+

### Migrations

Generate: `dotnet ef migrations add message -o Migrations` (from `Holtz.CQRS.Infraestructure` folder)
Apply: `dotnet ef database update` (from `Holtz.CQRS.Api` folder)

### TO DO list

- Add Middleware
- Add Unit tests
- Add integration tests
- Add support to Windows container
- Add swagger annotations
- Add notification
- Add FluentValidation
