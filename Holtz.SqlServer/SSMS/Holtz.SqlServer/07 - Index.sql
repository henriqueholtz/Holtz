USE [AdventureWorksDW2022]

SET STATISTICS Time ON
SET STATISTICS IO ON;

SELECT FirstName, LastName, EmailAddress FROM DimCustomer 
	WHERE LastName = 'Smith';




CREATE NONCLUSTERED INDEX [IX_New_DimCustomer_LastName] ON [dbo].[DimCustomer] ([LastName]) -- INCLUDE ([FirstName], [EmailAddress]);
--DROP INDEX [IX_New_DimCustomer_LastName] ON DimCustomer





SELECT FirstName, LastName, EmailAddress FROM DimCustomer WITH(INDEX(0)) -- Force to use the clustered index (force to run the same way before create the above index)
	WHERE LastName = 'Smith';
	
PRINT '************************************************************************************************************************************'

SELECT FirstName, LastName, EmailAddress FROM DimCustomer WITH(INDEX(IX_New_DimCustomer_LastName)) 
	WHERE LastName = 'Smith';

	
/*
--DECLARE @OrderDateKey INT = 20130101;

-- Query that will likely result in a table scan or index scan
SELECT SalesAmount FROM FactInternetSales WHERE OrderDateKey = 20130101;






-- Create a nonclustered index on OrderDateKey
CREATE NONCLUSTERED INDEX IX_FactInternetSales_OrderDateKey ON FactInternetSales (OrderDateKey);
--DROP INDEX IX_FactInternetSales_OrderDateKey ON FactInternetSales 








-- Query that will likely use the new index
SELECT SalesAmount FROM FactInternetSales WHERE OrderDateKey = @OrderDateKey;

-- Query forcing to use the older index (comparison purpose)
SELECT SalesAmount FROM FactInternetSales WITH(INDEX(PK_FactInternetSales_SalesOrderNumber_SalesOrderLineNumber)) WHERE OrderDateKey = @OrderDateKey;




*/
