USE [AdventureWorksDW2022]

/*

	1. Shouldn't consider Physical reads as performance improvement
		R: SQL Server ALWAYS use the buffer cache. Physical reads occurs sometimes just to put the content from disk to buffer cache.

	2. "Logical Reads" should be our guide
		R: Any improvement at "Logical Reads" is a performance upgrade!

*/
SET STATISTICS IO ON

SELECT TOP 100 * FROM FactInternetSales
	WHERE ProductKey > 300

SET STATISTICS IO OFF

/* Show about physical reads */
--DBCC DROPCLEANBUFFERS;
