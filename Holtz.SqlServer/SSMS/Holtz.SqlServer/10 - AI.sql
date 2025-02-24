USE [AdventureWorksDW2022]


/*
	Ask performance improvememts to LLM models (e.g: ChatGPT, DeepSeek, Gemini, etc). 

	See some prompt examples:

	1. "I have an SQL query for SQL Server that is running slow. Can you help me optimize it by following best practices? 
			Please consider indexing, query structure, execution plan, and performance improvements specific to SQL Server. Here is my query: {...}"

	--------------------------------------------------------------------------------------------------------------------------------------------------

	2. "Help me optimize this SQL Server query for better performance. 
		Here is the query: {...}
		
		Tables:
		  - Orders: OrderID (PK), CustomerID, OrderDate
		  - Customers: CustomerID (PK), Name
	
		Issues:
		- Slow performance (e.g., takes 5+ seconds).
		- Execution plan shows table scans.

	--------------------------------------------------------------------------------------------------------------------------------------------------

	3. "I have a SQL Server query that I want to optimize for better performance. Below are the details of the query, the database schema, 
		and the current performance issues I'm facing. Please analyze the query and suggest improvements based on SQL Server best practices, 
		such as indexing, query refactoring, execution plan analysis, and any other relevant optimizations. If possible, provide the optimized 
		query and explain the reasoning behind the changes.

		Query: {...}

		Database Schema
		  - Table Orders: Columns OrderID (PK), CustomerID, OrderDate, TotalAmount
		  - Table Customers: Columns CustomerID (PK), Name, Email, City
		  - Indexes: [List any existing indexes, e.g., IX_Orders_CustomerID on Orders(CustomerID)]

		Performance issues:
		  - The query takes too long to execute (e.g., 10+ seconds).
		  - High CPU or I/O usage is observed during execution.
		  - The execution plan shows issues like table scans, key lookups, or high-cost operators.

		Additional Context:
		  - SQL Server Version: [e.g., SQL Server 2019]
		  - Database Size: [e.g., 10 GB]
		  - Expected Output: [Describe what the query should return, e.g., "A list of orders with customer details for the last 30 days."]

		Please provide specific recommendations, such as:
		  - Indexes to add or modify.
		  - Query refactoring suggestions (e.g., avoiding cursors, reducing nested queries, using CTEs or window functions).
		  - Execution plan analysis and how to interpret it.
		  - Any configuration changes or server-level optimizations that might help.

*/

