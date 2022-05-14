SELECT mc.CountryCode, COUNT(m.MountainRange) AS [MountainRanges]
FROM MountainsCountries as mc
INNER JOIN Mountains as m ON mc.MountainId = m.Id
WHERE mc.CountryCode IN ('BG', 'US', 'RU')
GROUP BY mc.CountryCode