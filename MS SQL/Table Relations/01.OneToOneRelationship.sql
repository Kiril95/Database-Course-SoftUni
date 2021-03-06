CREATE TABLE Passports
(
	[PassportID] INT PRIMARY KEY,
	[PassportNumber] NVARCHAR(25) NOT NULL,
)
CREATE TABLE Persons
(
	[PersonID] INT IDENTITY(1,1) PRIMARY KEY,
	[FirstName] NVARCHAR(15) NOT NULL,
	[Salary] DECIMAL(10,2),
	[PassportID] INT FOREIGN KEY REFERENCES Passports(PassportID),
)
INSERT INTO Passports VALUES
(101, 'N34FG21B'),
(102, 'K65LO4R7'),
(103, 'ZE657QP2')

INSERT INTO Persons VALUES
('Roberto', 43300.00, 102),
('Tom', 56100.00, 103),
('Yana', 60200.00, 101)