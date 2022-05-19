CREATE PROCEDURE [usp_GetEmployeesFromTown] (@Town NVARCHAR(25)) as
SELECT FirstName, LastName FROM Employees as e
JOIN Addresses as a ON a.AddressID = e.AddressID
JOIN Towns as t ON t.TownID = a.TownID
WHERE t.Name = @Town

EXEC dbo.usp_GetEmployeesFromTown @Town = 'Sofia'