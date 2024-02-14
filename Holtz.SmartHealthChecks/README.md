# Holtz.SmartHealthChecks

- :heavy_check_mark: **.NET 8**
- :heavy_check_mark: **Docker support**
- :heavy_check_mark: **Swagger**

## Set up the environment

- Run SQL Server as docker container:

  ```
  docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Holtz@123" \
  -p 1433:1433 --name sql1 --hostname sql1 \
  -d \
  mcr.microsoft.com/mssql/server:2022-latest
  ```

- Execute the migrations with the following command:
  ```
  dotnet ef database update --project Holtz.SmartHealthChecks.Api/Holtz.SmartHealthChecks.Api.csproj
  ```
