
-- Get Author By ID
CREATE PROCEDURE sp_GetAuthorByID
    @AuthorID INT
AS
BEGIN
    SELECT * FROM Authors WHERE AuthorID = @AuthorID;
END;
GO

-- Get User By AuthorID
CREATE PROCEDURE sp_GetAuthorByPersonID
    @PersonID INT
AS
BEGIN
    SELECT * FROM Authors WHERE PersonID = @PersonID;
END;
GO

-- Get All Authors
CREATE PROCEDURE sp_GetAllAuthors
AS
BEGIN
    SELECT  Authors.AuthorID, FullName=People.FirstName + ' ' + People.LastName, People.DateOfBirth, GenderCaption=CASE WHEN People.Gender='M' THEN 'Male' WHEN People.Gender='F' THEN 'Female' END, People.Email, People.Phone, People.Address, People.City, Authors.Biography
		FROM      People INNER JOIN
                 Authors ON People.PersonID = Authors.PersonID ORDER BY AuthorID;
END;
GO

-- Get All Authors2
CREATE PROCEDURE sp_GetAllAuthors2
AS
BEGIN
    SELECT * from Authors ORDER BY AuthorID;
END;
GO

-- Add New Author
CREATE PROCEDURE sp_AddNewAuthor
    @PersonID INT,
    @Biography NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Authors (PersonID, Biography)
    VALUES (@PersonID, @Biography);
    
    -- Return newly created AuthorID
    SELECT SCOPE_IDENTITY() AS NewAuthorID;
END;
GO

-- Update Author
CREATE PROCEDURE sp_UpdateAuthor
    @AuthorID INT,
    @Biography NVARCHAR(MAX)
AS
BEGIN
    UPDATE Authors
    SET Biography = @Biography
    WHERE AuthorID = @AuthorID;
END;
GO

-- Delete Author
CREATE PROCEDURE sp_DeleteAuthor
    @AuthorID INT
AS
BEGIN
    DELETE FROM Authors WHERE AuthorID = @AuthorID;
END;
GO

-- Is Author Exist
CREATE PROCEDURE sp_IsAuthorExist
    @AuthorID INT
AS
BEGIN
    IF EXISTS(SELECT * FROM Authors WHERE AuthorID = @AuthorID)
        RETURN 1;  -- Author exists
    ELSE
        RETURN 0;  -- Author does not exist
END;
GO
