--CREATE DATABASE [Hotel]

CREATE TABLE Employees
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	FirstName NVARCHAR(15) NOT NULL,
	LastName NVARCHAR(15) NOT NULL,
	Title NVARCHAR(20),
	Notes NVARCHAR(MAX)
)
CREATE TABLE Customers
(
	AccountNumber INT PRIMARY KEY,
	FirstName NVARCHAR(15) NOT NULL,
	LastName NVARCHAR(15) NOT NULL,
	PhoneNumber INT NOT NULL,
	EmergencyName NVARCHAR(15),
	EmergencyNumber INT,
	Notes NVARCHAR(MAX)
)
CREATE TABLE RoomStatus
(
	RoomStatus NVARCHAR(20) PRIMARY KEY,
	Notes NVARCHAR(MAX),
)
CREATE TABLE RoomTypes
(
	RoomType NVARCHAR(20) PRIMARY KEY,
	Notes NVARCHAR(MAX),
)
CREATE TABLE BedTypes
(
	BedType NVARCHAR(20) PRIMARY KEY,
	Notes NVARCHAR(MAX),
)
CREATE TABLE Rooms
(
	RoomNumber INT PRIMARY KEY,
	RoomType NVARCHAR(20) FOREIGN KEY REFERENCES RoomTypes(RoomType),
	BedType NVARCHAR(20) FOREIGN KEY REFERENCES BedTypes(BedType),
	Rate DECIMAL(4,2),
	RoomStatus NVARCHAR(20) FOREIGN KEY REFERENCES RoomStatus(RoomStatus),
	Notes NVARCHAR(MAX)
)
CREATE TABLE Payments
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	PaymentDate DATE,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber),
	FirstDateOccupied DATE,
	LastDateOccupied DATE,
	TotalDays INT,
	AmountCharged DECIMAL(8,2),
	TaxRate DECIMAL(4,2),
	TaxAmount DECIMAL(4,2),
	PaymentTotal DECIMAL(10,2) NOT NULL,
	Notes NVARCHAR(MAX)
)
CREATE TABLE Occupancies
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	DateOccupied DATE,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber),
	RoomNumber INT FOREIGN KEY REFERENCES Rooms(RoomNumber),
	RateApplied DECIMAL(4,2),
	PhoneCharge BIT,
	Notes NVARCHAR(MAX)
)

INSERT INTO [Employees] VALUES
('Pesho', 'Peshov', NULL, NULL),
('Gosho', 'Goshov', NULL, NULL),
('Tosho', 'Toshov', NULL, NULL)
INSERT INTO [Customers] VALUES
(11, 'Donald', 'Trump', '071598', NULL, NULL, NULL),
(12, 'Boiko', 'Borisov', '0887468426', NULL, NULL, NULL),
(13, 'Johny', 'Depp', '3332098', NULL, NULL, NULL)
INSERT INTO [RoomStatus] VALUES
('FREE', NULL),
('OCCUPIED', NULL),
('Do not disturb', NULL)
INSERT INTO [RoomTypes] VALUES
('Single', NULL),
('Double', NULL),
('King', NULL)
INSERT INTO [BedTypes] VALUES
('Kingsize', NULL),
('Queen', NULL),
('Retro', NULL)
INSERT INTO [Rooms] VALUES
(23, 'King', 'Kingsize', NULL, 'Do not disturb', NULL),
(34, 'Single', 'Retro', NULL, 'OCCUPIED', NULL),
(89, 'Double', 'Queen', NULL, 'FREE', NULL)
INSERT INTO [Payments] (EmployeeId,PaymentDate,AccountNumber,TotalDays,AmountCharged,PaymentTotal) VALUES
(2, '2021-02-05', 11, 7, 2334.82, 2784.55),
(3, '2020-11-27', 12, 8, 6754.85, 7135.56),
(1, '2019-03-08', 13, 9, 12378.67, 12982.43)
INSERT INTO [Occupancies] (EmployeeId,AccountNumber,RoomNumber) VALUES
(2, 11, 23),
(3, 12, 34),
(1, 13, 89)