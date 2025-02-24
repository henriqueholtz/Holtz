/*
	
	Some of the most common performance improvements on MSSQL

	- Indexes
		- Index frequently queried columns
		- Avoid Excessive indexing
		- Monitor to rebuild and/or reorganize when needed (high fragmentation)
		- Filtered Index 

	- Use "SELECT TOP X" whenever as possible

	- Prefer to declare the columns you wan to instead of "SELECT * FROM ..."
	
	- Avoid using subqueries (prefer to use JOINs)

	- Minimize JOINs

	- Avoid using wildcards searches (e.g: {...} LIKE '%abc%')

	- Avoid using cursors

	- Avoid sorting

	- Use WITH(NOLOCK) when it's adequate

	- Avoid using functions on WHERE clauses. Example:
		- BAD    => SELECT * FROM Orders WHERE YEAR(OrderDate) = 2023;
		- BETTER => SELECT * FROM Orders WHERE OrderDate >= '2023-01-01' AND OrderDate < '2024-01-01';





*/

