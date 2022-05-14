SELECT TOP 5 e.EmployeeID, e.FirstName, p.Name as [ProjectName]
FROM Employees as e
LEFT OUTER JOIN EmployeesProjects as ep ON e.EmployeeID = ep.EmployeeID
INNER JOIN Projects as p ON p.ProjectID = ep.ProjectID
WHERE p.StartDate > '2002-08-13' AND p.EndDate IS NULL
ORDER BY e.EmployeeID