CREATE PROCEDURE usp_TransferMoney(@senderId INT, @receiverId INT, @amount DECIMAL(15,4)) AS
BEGIN TRANSACTION
	EXEC usp_WithdrawMoney @senderId, @amount;
	EXEC usp_DepositMoney @receiverId,@amount;
COMMIT

EXEC usp_TransferMoney 5,1,1000
SELECT * FROM Accounts 