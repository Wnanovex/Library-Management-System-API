
-- Get BookCopy By ID
CREATE PROCEDURE sp_GetBookCopyByID
    @CopyID INT
AS
BEGIN
    SELECT * FROM BookCopies WHERE CopyID = @CopyID;
END;
GO

-- Get All BookCopies
CREATE PROCEDURE sp_GetAllBookCopies
AS
BEGIN
    SELECT  BookCopies.CopyID, BookCopies.BookID, Books.Title, Categories.Name as CategoryName, People.FirstName + ' ' + People.LastName as Author, Books.Language, 
		Status=CASE WHEN BookCopies.Status=1 THEN 'Available' WHEN BookCopies.Status=2 THEN 'Issued' WHEN BookCopies.Status=3 THEN 'Damaged' WHEN BookCopies.Status=4 THEN 'Lost' END
	FROM      Books INNER JOIN
                 BookCopies ON Books.BookID = BookCopies.BookID INNER JOIN
                 Authors ON Books.AuthorID = Authors.AuthorID INNER JOIN
                 People ON Authors.PersonID = People.PersonID INNER JOIN
                 Categories ON Books.CategoryID = Categories.CategoryID ORDER BY CopyID;
END;
GO

-- Get All BookCopies2
CREATE PROCEDURE sp_GetAllBookCopies2
AS
BEGIN
    SELECT * from BookCopies ORDER BY CopyID;
END;
GO

-- Add New BookCopy
CREATE PROCEDURE sp_AddNewBookCopy
	@BookID INT,
    @Status SMALLINT = 1
AS
BEGIN
    INSERT INTO BookCopies (BookID, Status)
    VALUES (@BookID, @Status);
    
    -- Return newly created BookCopyID
    SELECT SCOPE_IDENTITY() AS NewBookCopyID;
END;
GO

-- Update BookCopy
CREATE PROCEDURE sp_UpdateBookCopy
    @CopyID INT,
	@BookID INT,
    @Status SMALLINT = 1
AS
BEGIN
    UPDATE BookCopies
    SET BookID = @BookID,
		Status = @Status
    WHERE CopyID = @CopyID;
END;
GO

-- Update BookCopy Status
CREATE PROCEDURE sp_UpdateBookCopyStatus
    @CopyID INT,
    @Status SMALLINT = 1
AS
BEGIN
    UPDATE BookCopies
    SET Status = @Status
    WHERE CopyID = @CopyID;
END;
GO

-- Delete BookCopy
CREATE PROCEDURE sp_DeleteBookCopy
    @CopyID INT
AS
BEGIN
    DELETE FROM BookCopies WHERE CopyID = @CopyID;
END;
GO


-- Is Book Issued
CREATE PROCEDURE sp_IsBookIssued
    @CopyID INT
AS
BEGIN
    IF EXISTS(SELECT * FROM BookCopies WHERE CopyID = @CopyID AND Status = 2)
        RETURN 1;
    ELSE
        RETURN 0; 
END;
GO

-- Is Book Issued
CREATE PROCEDURE sp_IsBookAvailable
    @CopyID INT
AS
BEGIN
    IF EXISTS(SELECT * FROM BookCopies WHERE CopyID = @CopyID AND Status = 1)
        RETURN 1;
    ELSE
        RETURN 0; 
END;
GO

