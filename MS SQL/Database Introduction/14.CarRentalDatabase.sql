CREATE DATABASE [CarRental]

CREATE TABLE Categories
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[CategoryName] NVARCHAR(25) NOT NULL,
	[DailyRate] DECIMAL(4,2),
	[WeeklyRate] DECIMAL(4,2),
	[MonthlyRate] DECIMAL(4,2),
	[WeekendRate] DECIMAL(4,2)
)
CREATE TABLE Cars
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[PlateNumber] NVARCHAR(8) NOT NULL,
	[Manufacturer] NVARCHAR(20) NOT NULL,
	[Model] NVARCHAR(20) NOT NULL,
	[CarYear] INT NOT NULL,
	[CategoryId] INT FOREIGN KEY REFERENCES Categories(Id),
	[Doors] INT,
	[Picture] VARBINARY(MAX),
	[Condition] NVARCHAR(10),
	[Available] BIT NOT NULL
)
CREATE TABLE Employees
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[FirstName] NVARCHAR(15) NOT NULL,
	[LastName] NVARCHAR(15) NOT NULL,
	[Title] NVARCHAR(20),
	[Notes] NVARCHAR(MAX)
)
CREATE TABLE Customers
(
	[Id] INT IDENTITY PRIMARY KEY,
	[DriverLicenceNumber] NVARCHAR(8) NOT NULL,
	[FullName] NVARCHAR(50) NOT NULL,
	[Address] NVARCHAR(100),
	[City] NVARCHAR(30),
	[ZIPCode] NVARCHAR(15),
	[Notes] NVARCHAR(MAX)
)
CREATE TABLE RentalOrders
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[EmployeeId] INT FOREIGN KEY REFERENCES Employees(Id),
	[CustomerId] INT FOREIGN KEY REFERENCES Customers(Id),
	[CarId] INT FOREIGN KEY REFERENCES Cars(Id),
	[TankLevel] INT,
	[KilometrageStart] INT,
	[KilometrageEnd] INT,
	[TotalKilometrage] INT,
	[StartDate] DATETIME2 ,
	[EndDate] DATETIME2,
	[TotalDays] INT,
	[RateApplied] DECIMAL(4,2),
	[TaxRate] DECIMAL(4,2),
	[OrderStatus] NVARCHAR(30) NOT NULL,
	[Notes] NVARCHAR(MAX)
)
INSERT INTO Categories VALUES
('Hatchback', NULL, NULL, NULL, NULL),
('Sedan', NULL, NULL, NULL, NULL),
('SUV', NULL, NULL, NULL, NULL)
INSERT INTO Cars VALUES
('??7736??', 'Audi', 'Q8', 2018, 3, 4, NULL, 'Perfect', 1),
('?A2437BA', 'Mitsubishi', 'Lancer', 2017, 2, 2, NULL, 'Perfect', 1),
('??1881EO', 'Honda', 'Civic', 2019, 1, 4, NULL, 'Good', 1)
INSERT INTO Employees VALUES
('Pesho', 'Peshev', NULL, NULL),
('Ico', 'Hazarta', NULL, NULL),
('Asencho', 'Taksito', NULL, NULL)
INSERT INTO Customers VALUES
('??0062TT', 'Todor Batkov', NULL, 'Sofia', NULL, NULL),
('CA4567CA', 'Zoran Toshkov', NULL, 'Kneja', NULL, NULL),
('C?0396HP', 'Nasko Mentata', NULL, 'Sungurlare', NULL, NULL)
INSERT INTO RentalOrders VALUES
(1, 2, 3, 100, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Pending', NULL),
(3, 2, 1 , 120, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Active', NULL),
(3, 1, 2 , 110, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Stolen', NULL)