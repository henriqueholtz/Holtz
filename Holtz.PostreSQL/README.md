# Holtz.PostreSQL

## Util commands

#### How to run PostgreSQL using docker

- `docker run --name holtz-postgresql -h holtz-postgresql -p 5432:5432 -e POSTGRES_PASSWORD=Holtz@Postgres! -d postgres`

#### How to manage the migrations

- Install the EF tool using dotnet: `dotnet tool install --global dotnet-ef`
- Create a new migration: `dotnet ef migrations add <nameForYourMigration>` (can be needed to add the arg `--project`)
- Apply a migration: ``
