CREATE PROCEDURE [usp_GetEmployeesSalaryAboveNumber] (@NumberInput DECIMAL(18,4)) as
SELECT FirstName, LastName FROM Employees
WHERE Salary >= @NumberInput

EXEC dbo.usp_GetEmployeesSalaryAboveNumber @NumberInput = 48100