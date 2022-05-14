SELECT TOP 5 e.EmployeeID, e.JobTitle, a.AddressID, a.AddressText 
FROM Employees as e
INNER JOIN Addresses as a ON a.AddressID = e.AddressID
ORDER BY e.AddressID