# CQRS & MediatR Pattern

## Requirements

- .NET 7+

### Migrations

Generate: `dotnet ef migrations add message -o Data` (from `Holtz.CQRS.Infraestructure` folder)
Apply: `dotnet ef database update`

### TO DO list

- Add Middleware
- Add Unit tests
- Add integration tests
- Add support to Windows container
- Add support to SQLite
- Add swagger annotations
- Add notification
- Add FluentValidation
