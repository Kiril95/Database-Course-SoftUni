CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4)) 
RETURNS VARCHAR(20) AS
BEGIN
	DECLARE @salaryLevel VARCHAR(20)
    IF (@salary < 30000) SET @salaryLevel = 'Low'
    ELSE IF (@salary BETWEEN 30000 AND 50000) SET @salaryLevel = 'Average'
	ELSE SET @salaryLevel = 'High'
 
    RETURN @salaryLevel
END

SELECT Salary, dbo.ufn_GetSalaryLevel(Salary) as [SalaryLevel] FROM Employees