CREATE PROC usp_WithdrawMoney (@accountId INT, @moneyAmount DECIMAL(12,4))
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
	SET Balance -= @moneyAmount
	WHERE Id = @accountId
COMMIT

EXEC usp_WithdrawMoney 5,25
SELECT * FROM Accounts