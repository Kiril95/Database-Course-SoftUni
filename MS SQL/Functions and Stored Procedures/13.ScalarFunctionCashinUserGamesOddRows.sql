CREATE FUNCTION ufn_CashInUsersGames(@game VARCHAR(40))
RETURNS TABLE AS
RETURN
( 
	SELECT SUM(Result.Cash) AS [SumCash] FROM
		(SELECT sub.Cash, ROW_NUMBER() OVER(PARTITION BY sub.NAME ORDER BY CASH DESC) AS [Ranking] FROM
			(SELECT ug.Cash, g.Name FROM UsersGames ug
				JOIN Games AS g ON g.Id = ug.GameId
				WHERE g.Name = @game
			) AS [Sub]
		) AS [Result]
	WHERE Result.Ranking % 2 = 1
)

SELECT * from dbo.ufn_CashInUsersGames('Love in a mist')