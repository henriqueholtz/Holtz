# Dapper

###### SQL command used to create the database and table

```
USE master;
GO

CREATE DATABASE HoltzDapper;
GO

USE HoltzDapper;
GO

CREATE TABLE dbo.Students (
    Id int PRIMARY KEY,
	FullName NVARCHAR(MAX) NOT NULL,
    IsActive BIT NOT NULL,
    BirthDate datetime NULL
);
```
