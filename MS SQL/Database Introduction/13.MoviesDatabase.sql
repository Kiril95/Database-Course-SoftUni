CREATE DATABASE [Movies]

CREATE TABLE Directors
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	DirectorName NVARCHAR(MAX) NOT NULL,
	Notes NVARCHAR(MAX)
)
CREATE TABLE Genres
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	GenreName NVARCHAR(MAX) NOT NULL,
	Notes NVARCHAR(MAX)
)
CREATE TABLE Categories
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	CategoryName NVARCHAR(MAX) NOT NULL,
	Notes NVARCHAR(MAX)
)
CREATE TABLE Movies
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Title NVARCHAR(MAX) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id),
	CopyrightYear INT NOT NULL,
	Length TIME,
	GenreId INT FOREIGN KEY REFERENCES Genres(Id),
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Rating DECIMAL(4,2),
	Notes NVARCHAR(MAX)
)
INSERT INTO [Directors] VALUES
('Kircho', NULL),
('Diana', NULL),
('Krasi', NULL),
('Tedi', NULL),
('Joro', NULL)
INSERT INTO [Genres] VALUES
('Fantasy', NULL),
('Action', NULL),
('Drama', NULL),
('Sci-fi', NULL),
('Adventure', NULL)
INSERT INTO [Categories] VALUES
('Series', NULL),
('Anime', NULL),
('Movies', NULL),
('Reality', NULL),
('Documentary', NULL)
INSERT INTO [Movies] VALUES
('Lord of the rings', 1, 2001, NULL, 1, 3, 10.00, NULL),
('Star Wars', 2, 1977, NULL, 4, 3, 9.50, NULL),
('The Witcher', 3, 2019, NULL, 1, 1, 9.50, NULL),
('Snowpiercer', 4, 2020, NULL, 2, 1, 10.00, NULL),
('Nightmare Alley', 5, 2022, NULL, 3, 3, 9.00, NULL)