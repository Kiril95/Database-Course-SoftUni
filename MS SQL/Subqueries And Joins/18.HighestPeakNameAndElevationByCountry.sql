SELECT TOP 5 Result.CountryName, 
	   ISNULL(Result.PeakName,'(no highest peak)') AS [Highest Peak Name], 
	   ISNULL(Result.Elevation,0) AS [Highest Peak Elevation],
	   ISNULL(Result.MountainRange,'(no mountain)') AS [Mountain]
FROM (SELECT C.CountryName, p.PeakName, p.Elevation, m.MountainRange,  
	 DENSE_RANK() OVER (PARTITION BY CountryName ORDER BY Elevation DESC) AS Ranking 
  FROM Countries AS c
  LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
  LEFT JOIN Mountains AS m ON mc.MountainId = m.Id
  LEFT JOIN Peaks AS p ON m.ID = p.MountainId) AS Result
WHERE Result.Ranking = 1
ORDER BY CountryName, PeakName