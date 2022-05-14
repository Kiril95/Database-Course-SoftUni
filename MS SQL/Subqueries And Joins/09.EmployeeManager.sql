SELECT e.EmployeeID, e.FirstName, e.ManagerID, emp.FirstName as [ManagerName]
FROM Employees as e
INNER JOIN Employees as emp ON e.ManagerID = emp.EmployeeID
WHERE e.ManagerID IN (3, 7)
ORDER BY e.EmployeeID