SELECT e.FirstName, e.LastName, e.HireDate, d.Name as [DeptName] 
FROM Employees as e
INNER JOIN Departments as d ON d.DepartmentID = e.DepartmentID
WHERE e.HireDate > '1999-01-01' AND d.Name = 'Finance' OR d.Name = 'Sales'
ORDER BY e.HireDate