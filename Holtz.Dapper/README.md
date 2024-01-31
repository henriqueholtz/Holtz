# Dapper

- :heavy_check_mark: **.NET 7**
- :heavy_check_mark: **Dapper**
- :heavy_check_mark: **Docker support**

###### SQL command used to create the database and table

```
USE master;
GO

CREATE DATABASE HoltzDapper;
GO

USE HoltzDapper;
GO

CREATE TABLE dbo.Students (
    Id int PRIMARY KEY IDENTITY(1,1),
	FullName NVARCHAR(MAX) NOT NULL,
    IsActive BIT NOT NULL,
    BirthDate datetime NULL
);
```
