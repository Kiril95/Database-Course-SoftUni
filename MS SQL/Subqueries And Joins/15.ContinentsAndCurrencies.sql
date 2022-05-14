SELECT ContinentCode, CurrencyCode, CurrencyUsage FROM 
	(SELECT c.ContinentCode, c.CurrencyCode, COUNT(c.CurrencyCode) AS [CurrencyUsage],
		DENSE_RANK() OVER (PARTITION BY ContinentCode ORDER BY COUNT(CurrencyCode) DESC) AS Ranking
	FROM Countries AS c
	GROUP BY c.ContinentCode, c.CurrencyCode) AS Result
WHERE Result.Ranking = 1 AND Result.CurrencyUsage > 1
ORDER BY ContinentCode 