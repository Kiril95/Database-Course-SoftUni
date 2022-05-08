--SELECT MountainRange, PeakName, Elevation FROM Mountains, Peaks
--WHERE MountainId = '17' AND Mountains.Id = 17
--ORDER BY Elevation DESC

SELECT Mountains.MountainRange, Peaks.PeakName, Peaks.Elevation FROM Mountains
JOIN Peaks ON Peaks.MountainId = Mountains.Id
WHERE MountainRange = 'Rila'
ORDER BY Elevation DESC