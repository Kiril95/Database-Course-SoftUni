CREATE PROC usp_DepositMoney (@accountId INT, @moneyAmount DECIMAL(12,4))
AS
	BEGIN TRANSACTION
	DECLARE @account INT = (SELECT ID FROM Accounts WHERE ID = @accountId)
	if (@account IS NULL)
	BEGIN
		ROLLBACK
		RAISERROR('There is not a user with this Id!', 13, 1)
		RETURN
	END

	if (@moneyAmount < 0)
	BEGIN
		ROLLBACK
		RAISERROR('The amount has to be a positive number!', 13, 1)
		RETURN
	END

	UPDATE Accounts
	SET Balance += @moneyAmount
	WHERE Id = @account
COMMIT

EXEC usp_DepositMoney 1,10
SELECT * FROM Logs