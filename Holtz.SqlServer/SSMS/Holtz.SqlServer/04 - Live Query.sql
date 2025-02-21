USE [AdventureWorks2022]

/*
	"Live Query Statistics": Real time insights:
		- Percentage of completion
		- Number of rows processed
		- Blocking or waiting operators
		- Suitable to long queries


	1. Click to turn on "Include Live Query Statistics"
	
*/


SELECT * FROM Sales.SalesOrderDetail
	ORDER BY LineTotal DESC;


SELECT soh.SalesOrderID, c.CustomerID, c.PersonID, SUM(sod.LineTotal) AS TotalSales
	FROM Sales.SalesOrderHeader soh
	JOIN Sales.SalesOrderDetail sod ON soh.SalesOrderID = sod.SalesOrderID
	JOIN Sales.Customer c ON soh.CustomerID = c.CustomerID
	GROUP BY soh.SalesOrderID, c.CustomerID, c.PersonID
	ORDER BY TotalSales DESC;

SELECT soh.SalesOrderID, soh.OrderDate, c.CustomerID, 
       (SELECT COUNT(*) FROM Sales.SalesOrderDetail sod WHERE sod.SalesOrderID = soh.SalesOrderID) AS ItemCount
	FROM Sales.SalesOrderHeader soh
	JOIN Sales.Customer c ON soh.CustomerID = c.CustomerID;