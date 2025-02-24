--RESTORE DATABASE AdventureWorks2022 FROM DISK = '/var/opt/mssql/backup/AdventureWorks2022.bak' WITH MOVE 'AdventureWorks2022' TO '/var/opt/mssql/data/AdventureWorks2022.mdf', MOVE 'AdventureWorks2022_log' TO '/var/opt/mssql/data/AdventureWorks2022_log.ldf', REPLACE;
--GO

RESTORE DATABASE AdventureWorksDW2022 FROM DISK = '/var/opt/mssql/backup/AdventureWorksDW2022.bak' WITH MOVE 'AdventureWorksDW2022' TO '/var/opt/mssql/data/AdventureWorksDW2022.mdf', MOVE 'AdventureWorksDW2022_log' TO '/var/opt/mssql/data/AdventureWorksDW2022_log.ldf', REPLACE;
GO