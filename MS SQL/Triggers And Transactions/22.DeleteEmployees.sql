CREATE TABLE Deleted_Employees 
(
	EmployeeId INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(30),
	LastName NVARCHAR(30),
	MiddleName NVARCHAR(30),
	JobTitle NVARCHAR(40),
	DepartmentId INT,
	Salary DECIMAL(13,2)
)

CREATE TRIGGER tr_DeletedEmployeesTrigger
ON Employees FOR DELETE AS
	INSERT INTO Deleted_Employees (FirstName, LastName, MiddleName, JobTitle, DepartmentId, Salary)
	SELECT FirstName, LastName, MiddleName, JobTitle, DepartmentID, Salary FROM deleted