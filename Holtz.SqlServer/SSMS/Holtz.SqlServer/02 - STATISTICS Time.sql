USE [AdventureWorksDW2022]

/*

	-> CPU time and/vs elapsed time

*/

SET STATISTICS Time ON

SELECT TOP 100 * FROM FactInternetSales
	WHERE ProductKey > 300

SET STATISTICS Time OFF