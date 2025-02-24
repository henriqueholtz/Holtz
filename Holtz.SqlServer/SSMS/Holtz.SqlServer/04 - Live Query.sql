USE [AdventureWorksDW2022]

/*
	"Live Query Statistics": Real time insights:
		- Percentage of completion
		- Number of rows processed
		- Blocking or waiting operators
		- Suitable to long queries


	1. Click to turn on "Include Live Query Statistics"
	
*/


SELECT * FROM FactProductInventory fpi
	JOIN  DimProduct dp ON fpi.ProductKey = dp.ProductKey
	WHERE YEAR(MovementDate) = 2012

