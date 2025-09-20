
-- Get Person By ID
CREATE PROCEDURE sp_GetPersonByID
    @PersonID INT
AS
BEGIN
    SELECT * FROM People WHERE PersonID = @PersonID;
END;
GO

-- Get All People
CREATE PROCEDURE sp_GetAllPeople
AS
BEGIN
    SELECT * FROM People ORDER BY PersonID;
END;
GO

-- Add New Person
CREATE PROCEDURE sp_AddNewPerson
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DateOfBirth DATE = NULL,
    @Gender CHAR(1) = NULL,
    @Email NVARCHAR(100) = NULL,
    @Phone NVARCHAR(20) = NULL,
    @Address NVARCHAR(150) = NULL,
    @City NVARCHAR(50) = NULL
AS
BEGIN
    INSERT INTO People (FirstName, LastName, DateOfBirth, Gender, Email, Phone, Address, City, UpdatedAt)
    VALUES (@FirstName, @LastName, @DateOfBirth, @Gender, @Email, @Phone, @Address, @City, GETDATE());
    
    -- Return newly created PersonID
    SELECT SCOPE_IDENTITY() AS NewPersonID;
END;
GO

-- Update Person
CREATE PROCEDURE sp_UpdatePerson
    @PersonID INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DateOfBirth DATE = NULL,
    @Gender CHAR(1) = NULL,
    @Email NVARCHAR(100) = NULL,
    @Phone NVARCHAR(20) = NULL,
    @Address NVARCHAR(150) = NULL,
    @City NVARCHAR(50) = NULL
AS
BEGIN
    UPDATE People
    SET FirstName = @FirstName,
        LastName = @LastName,
        DateOfBirth = @DateOfBirth,
        Gender = @Gender,
        Email = @Email,
        Phone = @Phone,
        Address = @Address,
        City = @City,
        UpdatedAt = GETDATE()
    WHERE PersonID = @PersonID;
END;
GO

-- Delete Person
CREATE PROCEDURE sp_DeletePerson
    @PersonID INT
AS
BEGIN
    DELETE FROM People WHERE PersonID = @PersonID;
END;
GO

-- Is Person Exist
CREATE PROCEDURE sp_IsPersonExist
    @PersonID INT
AS
BEGIN
    IF EXISTS(SELECT * FROM People WHERE PersonID = @PersonID)
        RETURN 1;  -- Person exists
    ELSE
        RETURN 0;  -- Person does not exist
END;
GO
