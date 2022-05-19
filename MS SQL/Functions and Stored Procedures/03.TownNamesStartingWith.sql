CREATE PROCEDURE [usp_GetTownsStartingWith] (@Word NVARCHAR(25)) as
SELECT [Name] FROM Towns
WHERE [Name] LIKE @Word + '%'

EXEC dbo.usp_GetTownsStartingWith @Word = 'b'