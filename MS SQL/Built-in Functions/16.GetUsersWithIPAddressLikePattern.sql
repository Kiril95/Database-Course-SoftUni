SELECT Username, IpAddress FROM Users
WHERE IpAddress LIKE '___.1%.%.___'
ORDER BY Username

-- % wildcard matches zero or more characters
-- _ wildcard matches exactly one character of any type