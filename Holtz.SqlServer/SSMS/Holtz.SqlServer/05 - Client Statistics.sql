USE [AdventureWorksDW2022]

/*
	"Client Statistics": Client-side metrics
		- AVG and cumulative execution time
		- Network Statistics
		- Time spent by the client processing the query results.
		- Wait time on the client side
		- Comparison Across Runs


	=> Click to turn on "Include Client Statistics" (Shift + Alt + S)
	
*/



SELECT TOP 100 * FROM FactInternetSales
	WHERE ProductKey > 300
