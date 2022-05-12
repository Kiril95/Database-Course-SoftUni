SELECT CountryName, IsoCode FROM Countries
WHERE UPPER(CountryName) LIKE UPPER('%A%A%A%')
ORDER BY IsoCode