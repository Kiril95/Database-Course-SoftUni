CREATE TABLE NotificationEmails 
(
	Id INT PRIMARY KEY IDENTITY ,
	Recipient INT FOREIGN KEY REFERENCES Accounts(Id),
	[Subject] NVARCHAR(40),
	Body NVARCHAR(MAX)
)

CREATE TRIGGER TR_AddEmailOnNewLogInsertion ON Logs FOR INSERT 
AS 
DECLARE @accountId INT = (SELECT TOP(1) AccountId FROM inserted)
DECLARE @oldSum DECIMAL(12,2) = (SELECT TOP(1) OldSum FROM inserted)
DECLARE @newSum DECIMAL(12,2) = (SELECT TOP(1) NewSum FROM inserted)

INSERT INTO NotificationEmails (Recipient,Subject, Body) VALUES
(
	@accountId,
	'Balance change for account: ' + CAST(@accountId AS NVARCHAR(20)),
	'On ' + CONVERT(NVARCHAR(30),GETDATE(),103) + ' your balance was changed from '
	+ CAST(@oldSum AS NVARCHAR(20)) + ' to ' + CAST(@newSum AS NVARCHAR(20))
)

SELECT * FROM NotificationEmails