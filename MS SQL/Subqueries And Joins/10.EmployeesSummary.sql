SELECT TOP 50 e.EmployeeID, CONCAT(e.FirstName, ' ', e.LastName) as [EmployeeName], 
	CONCAT(emp.FirstName, ' ', emp.LastName) as [ManagerName], d.Name as [DepartmentName]
FROM Employees as e
INNER JOIN Employees as emp ON e.ManagerID = emp.EmployeeID
INNER JOIN Departments as d ON d.DepartmentID = e.DepartmentID
ORDER BY e.EmployeeID