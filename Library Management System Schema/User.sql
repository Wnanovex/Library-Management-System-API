
-- Get User By ID
CREATE PROCEDURE sp_GetUserByID
    @UserID INT
AS
BEGIN
    SELECT * FROM Users WHERE UserID = @UserID;
END;
GO

-- Get User By PersonID
CREATE PROCEDURE sp_GetUserByPersonID
    @PersonID INT
AS
BEGIN
    SELECT * FROM Users WHERE PersonID = @PersonID;
END;
GO

-- Get User By Username And Password
CREATE PROCEDURE sp_GetUserByUsernameAndPassword
    @Username NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    SELECT * FROM Users WHERE Username = @Username and Password = @Password;
END;
GO

-- Get All Users
CREATE PROCEDURE sp_GetAllUsers
AS
BEGIN
    SELECT  Users.UserID, People.FirstName, People.LastName, People.DateOfBirth, GenderCaption=CASE WHEN People.Gender='M' THEN 'Male' WHEN People.Gender='F' THEN 'Female' END, People.Email, People.Phone, People.Address, People.City, Users.Username, Users.Role, Users.IsActive
		FROM      People INNER JOIN
                 Users ON People.PersonID = Users.PersonID ORDER BY UserID;
END;
GO

-- Get All Users2
CREATE PROCEDURE sp_GetAllUsers2
AS
BEGIN
    SELECT * from Users ORDER BY UserID;
END;
GO

-- Add New User
CREATE PROCEDURE sp_AddNewUser
    @PersonID INT,
    @Username NVARCHAR(50),
    @Password NVARCHAR(50),
    @Role SMALLINT,
    @IsActive SMALLINT
AS
BEGIN
    INSERT INTO Users (PersonID, Username, Password, Role, IsActive)
    VALUES (@PersonID, @Username, @Password, @Role, @IsActive);
    
    -- Return newly created UserID
    SELECT SCOPE_IDENTITY() AS NewUserID;
END;
GO

-- Update User
CREATE PROCEDURE sp_UpdateUser
    @UserID INT,
    @Username NVARCHAR(50),
    @Password NVARCHAR(50),
    @Role SMALLINT,
    @IsActive SMALLINT
AS
BEGIN
    UPDATE Users
    SET Username = @Username,
		Password = @Password,
        Role = @Role,
        IsActive = @IsActive
    WHERE UserID = @UserID;
END;
GO

-- Change Password
CREATE PROCEDURE sp_ChangePassword
    @UserID INT,
    @Password NVARCHAR(50)
AS
BEGIN
    UPDATE Users
    SET Password = @Password
    WHERE UserID = @UserID;
END;
GO

-- Delete User
CREATE PROCEDURE sp_DeleteUser
    @UserID INT
AS
BEGIN
    DELETE FROM Users WHERE UserID = @UserID;
END;
GO

-- Is User Exist
CREATE PROCEDURE sp_IsUserExist
    @UserID INT
AS
BEGIN
    IF EXISTS(SELECT * FROM Users WHERE UserID = @UserID)
        RETURN 1;  -- User exists
    ELSE
        RETURN 0;  -- User does not exist
END;
GO
