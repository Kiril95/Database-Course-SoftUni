SELECT mc.CountryCode, m.MountainRange, p.PeakName, p.Elevation
FROM Peaks as p
INNER JOIN Mountains as m ON p.MountainId = m.Id
INNER JOIN MountainsCountries as mc ON mc.MountainId = m.Id
WHERE mc.CountryCode = 'BG' AND p.Elevation > 2835
ORDER BY p.Elevation DESC