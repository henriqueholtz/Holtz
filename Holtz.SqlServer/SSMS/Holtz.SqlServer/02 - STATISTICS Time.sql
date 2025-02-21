USE [AdventureWorks2022]

/*

	-> CPU time and/vs elapsed time

*/

SET STATISTICS Time ON

SELECT * FROM Production.Product 
	WHERE ProductID > 740

SET STATISTICS Time OFF