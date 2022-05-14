SELECT e.EmployeeID, e.FirstName,
CASE 
	WHEN p.StartDate >= '2005-01-01' THEN NULL
	ELSE p.Name
	END AS [ProjectName]
FROM Employees as e
INNER JOIN EmployeesProjects as ep ON e.EmployeeID = ep.EmployeeID
INNER JOIN Projects as p ON p.ProjectID = ep.ProjectID
WHERE e.EmployeeID = 24