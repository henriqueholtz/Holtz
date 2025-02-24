USE [AdventureWorksDW2022]
-- Example 1

SET STATISTICS IO ON
SET STATISTICS Time ON



-- Inefficient Query (Poor Performance, with 2 subqueries)
SELECT c.FirstName, c.LastName, SUM(fs.SalesAmount) AS TotalSales
FROM DimCustomer AS c
	JOIN FactInternetSales AS fs ON c.CustomerKey = fs.CustomerKey
		WHERE c.CustomerKey IN (
			SELECT CustomerKey FROM DimCustomer WHERE GeographyKey IN (
				SELECT GeographyKey FROM DimGeography WHERE EnglishCountryRegionName = 'United States'
			)
    )
GROUP BY c.FirstName, c.LastName;


PRINT '************************************************************************************************************************************'

-- Optimized Query (Better Performance)
SELECT c.FirstName, c.LastName, SUM(fs.SalesAmount) AS TotalSales
FROM DimCustomer AS c
JOIN FactInternetSales AS fs ON c.CustomerKey = fs.CustomerKey
JOIN DimGeography AS g ON c.GeographyKey = g.GeographyKey
WHERE g.EnglishCountryRegionName = 'United States'
GROUP BY c.FirstName, c.LastName;























/*
USE [AdventureWorksDW2022]
-- Example 2
SELECT 
    c.CustomerKey,
    (SELECT SUM(fis.SalesAmount) 
     FROM FactInternetSales fis 
     WHERE fis.CustomerKey = c.CustomerKey) AS TotalSalesAmount,
    (SELECT COUNT(*) 
     FROM FactInternetSales fis 
     WHERE fis.CustomerKey = c.CustomerKey) AS TotalTransactions,
    (SELECT AVG(fis.SalesAmount) 
     FROM FactInternetSales fis 
     WHERE fis.CustomerKey = c.CustomerKey) AS AvgSalesAmount
FROM 
    DimCustomer c
WHERE 
    c.CustomerKey IN (SELECT DISTINCT fis.CustomerKey FROM FactInternetSales fis);




SELECT 
    c.CustomerKey,
    SUM(fis.SalesAmount) AS TotalSalesAmount,
    COUNT(fis.SalesAmount) AS TotalTransactions,
    AVG(fis.SalesAmount) AS AvgSalesAmount
FROM 
    DimCustomer c
JOIN 
    FactInternetSales fis ON c.CustomerKey = fis.CustomerKey
GROUP BY 
    c.CustomerKey;
	*/
	

/*
-- Example 3
USE [AdventureWorksDW2022]
SELECT 
    fis.SalesOrderNumber,
    fis.SalesAmount,
    (SELECT TOP 1 fis2.SalesAmount FROM FactInternetSales fis2 WHERE fis2.CustomerKey = fis.CustomerKey ORDER BY fis2.SalesAmount DESC) AS MaxSalesAmount
FROM FactInternetSales fis ORDER BY fis.SalesOrderNumber;

SELECT 
    fis.SalesOrderNumber,
    fis.SalesAmount,
    agg.MaxSalesAmount
FROM FactInternetSales fis
	JOIN 
		(SELECT CustomerKey, MAX(SalesAmount) AS MaxSalesAmount FROM FactInternetSales GROUP BY CustomerKey) agg 
	 ON fis.CustomerKey = agg.CustomerKey
ORDER BY fis.SalesOrderNumber;

*/

