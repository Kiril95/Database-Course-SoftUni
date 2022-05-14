SELECT TOP 5 c.CountryName, MAX(p.Elevation) as [HighestPeakElevation], MAX(r.Length) as [LongestRiverLenght]
FROM Countries as c
	LEFT JOIN MountainsCountries as mc ON mc.CountryCode = c.CountryCode
	LEFT JOIN Peaks as p ON p.MountainId = mc.MountainId
	LEFT JOIN CountriesRivers as cr ON cr.CountryCode = c.CountryCode
	LEFT JOIN Rivers as r ON r.Id = cr.RiverId
GROUP BY c.CountryName
ORDER BY HighestPeakElevation DESC, LongestRiverLenght DESC, c.CountryName