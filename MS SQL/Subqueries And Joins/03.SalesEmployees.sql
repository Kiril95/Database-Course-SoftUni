SELECT e.EmployeeID, e.FirstName, e.LastName, d.Name as [Department Name] 
FROM Employees as e
INNER JOIN Departments as d ON d.DepartmentID = e.DepartmentID
WHERE d.Name = 'Sales'
ORDER BY e.EmployeeID