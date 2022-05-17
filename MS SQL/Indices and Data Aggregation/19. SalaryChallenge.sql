SELECT TOP 10 e.FirstName, e.LastName, e.DepartmentID
	FROM Employees AS e
	WHERE Salary > (SELECT AVG(Salary) AS [Avg] FROM Employees AS emp
		WHERE emp.DepartmentID = e.DepartmentID
		GROUP BY DepartmentID) 