USE [AdventureWorks2022]

/*
	"Client Statistics": Client-side metrics
		- AVG and cumulative execution time
		- Network Statistics
		- Time spent by the client processing the query results.
		- Wait time on the client side
		- Comparison Across Runs


	=> Click to turn on "Include Client Statistics" (Shift + Alt + S)
	
*/



SELECT * FROM Person.Person
	WHERE BusinessEntityID > 7500
