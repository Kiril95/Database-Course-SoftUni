CREATE TABLE [Users]
(
	Id BIGINT IDENTITY(1,1) PRIMARY KEY,
	Username VARCHAR(30) NOT NULL,
	Password VARCHAR(26) NOT NULL,
	ProfilePicture VARBINARY(MAX),
	LastLoginTime TIME,
	IsDeleted BIT
)
INSERT INTO [Users] VALUES
('Kiko95', '12356', NULL, NULL, 0),
('Kiky96', '12345', NULL, NULL, 0),
('Kiki97', '1234', NULL, NULL, 0),
('Kikq98', '123', NULL, NULL, 0),
('Kiku99', '12', NULL, NULL, 0)