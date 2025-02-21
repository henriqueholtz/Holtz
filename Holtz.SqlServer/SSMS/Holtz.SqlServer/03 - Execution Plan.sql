USE [AdventureWorks2022]

/*

	"Actual Execution Plan": Detailed execution plan
		- Actual steps taken by SQL Server Engine
		- Actual row counts
		- CPU usage
		- I/O costs
		- Operations taken (like table scan, index seek, index scan)
		- Optimal when looking for query optmization

	1. Click to turn on "Include Actual Execution Plan" (or press CTRL + M)
	
	- TODO: Add most common scenarios like index seek, index scan, table scan

*/


-- Query with Index Seek
SELECT BusinessEntityID, FirstName, LastName
	FROM Person.Person
	WHERE LastName = 'Smith';


-- Query with Table Scan
SELECT BusinessEntityID, FirstName, LastName
	FROM Person.Person
	WHERE MiddleName = 'A';



-- Query with Merge joib, index scan, sort and index seek
SELECT p.BusinessEntityID, p.FirstName, p.LastName, e.JobTitle
	FROM Person.Person p
	INNER JOIN HumanResources.Employee e ON p.BusinessEntityID = e.BusinessEntityID
	WHERE p.LastName = 'Smith';



-- Nested loops, index scan and index seek
SELECT p.BusinessEntityID, p.FirstName, p.LastName, e.JobTitle
	FROM Person.Person p
	INNER JOIN HumanResources.Employee e ON p.BusinessEntityID = e.BusinessEntityID;

