# Holtz.PostreSQL

- :heavy_check_mark: **.NET 8**
- :heavy_check_mark: **PostgreSQL**
- :heavy_check_mark: **Entity Framework Core**
- :heavy_check_mark: **Code First approach**
- :heavy_check_mark: **Docker support**
- :heavy_check_mark: **Swagger**
- :heavy_check_mark: **Integration Tests**

## Util commands

#### How to run PostgreSQL using docker

```
docker run --name holtz-postgresql -h holtz-postgresql -p 5432:5432 -e POSTGRES_PASSWORD=Holtz@Postgres! -d postgres
```

#### How to manage the migrations

- Install the EF tool using dotnet:

  `dotnet tool install --global dotnet-ef`

- Create a new migration:

  `dotnet ef migrations add <nameForYourMigration> --project Holtz.PostreSQL.Api/Holtz.PostreSQL.Api.csproj`

- Apply a migration:

  `dotnet ef database update --project Holtz.PostreSQL.Api/Holtz.PostreSQL.Api.csproj`
