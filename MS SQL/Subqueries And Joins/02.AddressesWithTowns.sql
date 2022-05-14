SELECT TOP 50 e.FirstName, e.LastName, t.Name as [Town], a.AddressText as [AdressText]
FROM Employees as e
INNER JOIN Addresses as a ON a.AddressID = e.AddressID
INNER JOIN Towns as t ON t.TownID = a.TownID
ORDER BY e.FirstName, e.LastName