SELECT TOP 5 c.CountryName, r.RiverName
FROM Countries as c
LEFT OUTER JOIN CountriesRivers cr ON cr.CountryCode = c.CountryCode
LEFT OUTER JOIN Rivers as r ON r.Id = cr.RiverId
WHERE c.ContinentCode = 'AF'
ORDER BY c.CountryName