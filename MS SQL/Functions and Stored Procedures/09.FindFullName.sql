CREATE PROCEDURE usp_GetHoldersFullName as
SELECT CONCAT(FirstName, ' ', LastName) as [FullName] FROM AccountHolders

EXEC dbo.usp_GetHoldersFullName