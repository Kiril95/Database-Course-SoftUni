CREATE TABLE [People]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(200) NOT NULL,
	Picture VARBINARY(MAX),
	Height DECIMAl(3,2),
	Weight DECIMAL(5,2),
	Gender NCHAR(1) NOT NULL,
	Birthdate DATETIME2 NOT NULL,
	Biography NVARCHAR(MAX),
)
INSERT INTO [People] VALUES
('Kiko', NULL, 1.78, 78, 'm', '1995-08-13', 'cool as a cucumber'),
('Miko', NULL, 1.77, 77, 'm', '1996-12-20', NULL),
('Piko', NULL, 1.76, 77, 'm', '1997-08-07', 'yeah'),
('Diko', NULL, 1.75, 77, 'm', '1998-03-05', NULL),
('CHIKO', NULL, 1.74, 107, 'm', '1999-06-14', 'nope')