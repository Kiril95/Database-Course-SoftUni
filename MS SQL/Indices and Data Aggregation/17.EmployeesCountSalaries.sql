SELECT COUNT(Salary) as [Count] FROM Employees
WHERE ManagerID IS NULL
GROUP BY ManagerID