USE [AdventureWorksDW2022]

/*

	"Actual Execution Plan": Detailed execution plan
		- Optimal when looking for query optmization
		- Actual steps taken by SQL Server Engine
	Show:
		- Actual row counts
		- CPU usage
		- I/O costs
		- Query cost
		- Operations taken (like table scan, index seek, index scan)

	1. Click to turn on "Include Actual Execution Plan" (or press CTRL + M)
*/


-- Query with Index Seek
SELECT ProductKey, EnglishProductName FROM DimProduct WHERE ProductKey = 123;


-- Query with Index Scan
SELECT * FROM DimProduct WHERE Color = 'Black';

-- Query to simulate a table scan
SELECT * INTO dbo.FactInternetSales_Heap FROM dbo.FactInternetSales;
SELECT * FROM dbo.FactInternetSales_Heap;


-- Query wutg Nested Loops, index scan and index seek
SELECT TOP 1000 * FROM FactProductInventory fpi
	JOIN  DimProduct dp ON fpi.ProductKey = dp.ProductKey
	WHERE YEAR(MovementDate) = 2012
	
