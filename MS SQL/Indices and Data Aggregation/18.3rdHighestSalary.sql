SELECT DepartmentID, [ThirdHighestSalary]
FROM (SELECT e.DepartmentID, MAX(e.Salary) AS [ThirdHighestSalary],
	DENSE_RANK() OVER(PARTITION BY DepartmentID ORDER BY Salary DESC) AS Ranking	
		FROM Employees AS e
		GROUP BY DepartmentID, Salary) AS [MaxSalaryQuery]
WHERE Ranking = 3