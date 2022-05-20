CREATE PROCEDURE usp_GetHoldersWithBalanceHigherThan(@balance DECIMAL(10,2)) as
SELECT FirstName, LastName FROM AccountHolders ah
JOIN Accounts as a ON a.AccountHolderId = ah.Id
GROUP BY FirstName, LastName
HAVING SUM(a.Balance) > @balance
ORDER BY FirstName, LastName

EXEC dbo.usp_GetHoldersWithBalanceHigherThan 2000