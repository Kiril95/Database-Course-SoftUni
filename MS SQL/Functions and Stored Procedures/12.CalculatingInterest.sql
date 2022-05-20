CREATE PROCEDURE usp_CalculateFutureValueForAccount(@id INT, @rate FLOAT) as
SELECT 
	ah.id as [Account Id], 
	ah.FirstName,
	ah.LastName, 
	a.Balance as [Current Balance],
	dbo.ufn_CalculateFutureValue(a.Balance, @rate, 5) as [Balance in 5 years]
FROM AccountHolders ah
JOIN Accounts as a ON a.AccountHolderId = ah.Id
WHERE a.Id = @id

EXEC usp_CalculateFutureValueForAccount 2,0.1