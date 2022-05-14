SELECT TOP 3 e.EmployeeID, e.FirstName
FROM Employees as e
LEFT OUTER JOIN EmployeesProjects as ep ON e.EmployeeID = ep.EmployeeID
WHERE ep.ProjectID IS NULL
ORDER BY e.EmployeeID