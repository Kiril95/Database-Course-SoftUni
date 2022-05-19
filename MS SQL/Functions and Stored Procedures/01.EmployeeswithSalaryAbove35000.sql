CREATE PROCEDURE [usp_GetEmployeesSalaryAbove35000] as
SELECT FirstName, LastName FROM Employees
WHERE Salary > 35000

EXEC dbo.usp_GetEmployeesSalaryAbove35000