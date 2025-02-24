# Holtz.SqlServer

### Requirements

- Docker
- SQL Server Management Studio (SSMS)

### Step by step

1. Download the AdventureWorksDW2022 database ([ref](https://learn.microsoft.com/en-us/sql/samples/adventureworks-install-configure?view=sql-server-ver16&tabs=ssms)) and paste the file `AdventureWorksDW2022.bak` into `./bak-files`
2. Run `docker compose up` to spin up the SQL Server and restore the database from the above mentioned file
3. Open [./SSMS/Holtz.SqlServer.ssmssln](./SSMS/Holtz.SqlServer.ssmssln) solution on SSMS
