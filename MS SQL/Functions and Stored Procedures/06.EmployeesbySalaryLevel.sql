CREATE PROCEDURE usp_EmployeesBySalaryLevel(@salaryLevel NVARCHAR(10)) as
SELECT FirstName, LastName FROM Employees
WHERE dbo.ufn_GetSalaryLevel(Salary) = @salaryLevel

EXEC dbo.usp_EmployeesBySalaryLevel 'High'