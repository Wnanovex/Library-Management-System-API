
-- Get Book By ID
CREATE PROCEDURE sp_GetBookByID
    @BookID INT
AS
BEGIN
    SELECT * FROM Books WHERE BookID = @BookID;
END;
GO

-- Get Books By CategoryID
CREATE PROCEDURE sp_GetBooksByCategoryID
    @CategoryID INT
AS
BEGIN
    SELECT * FROM Books WHERE CategoryID = @CategoryID;
END;
GO

-- Get All Books
CREATE PROCEDURE sp_GetAllBooks
AS
BEGIN
    SELECT  Books.BookID, Books.Title, Books.ISBN, Categories.Name as CategoryName, People.FirstName + ' ' + People.LastName as AuthorFullName, Books.Edition, Books.PublishedYear, Books.Language, Books.Description, Books.ShelfLocation, Books.DailyRate, Books.ImageName
		  FROM      Books INNER JOIN
                 Categories ON Books.CategoryID = Categories.CategoryID INNER JOIN
                 Authors ON Books.AuthorID = Authors.AuthorID INNER JOIN
                 People ON Authors.PersonID = People.PersonID
		  ORDER BY Title;
END;
GO

-- Get All Books2
CREATE PROCEDURE sp_GetAllBooks2
AS
BEGIN
    SELECT * from Books ORDER BY BookID;
END;
GO

-- Add New Book
CREATE PROCEDURE sp_AddNewBook
    @Title NVARCHAR(255),
	@ISBN NVARCHAR(20),
    @CategoryID INT,
	@AuthorID INT,
    @Edition NVARCHAR(50),
    @PublishedYear INT,
    @Language NVARCHAR(50),
	@Description NVARCHAR(MAX),
	@ShelfLocation NVARCHAR(50),
	@DailyRate DECIMAL(10, 2),
	@ImageName NVARCHAR(555)
AS
BEGIN
    INSERT INTO Books (Title, ISBN, CategoryID, AuthorID, Edition, PublishedYear, Language, Description, ShelfLocation, DailyRate, ImageName)
    VALUES (@Title, @ISBN, @CategoryID, @AuthorID, @Edition, @PublishedYear ,@Language ,@Description, @ShelfLocation, @DailyRate, @ImageName);
    
    -- Return newly created BookID
    SELECT SCOPE_IDENTITY() AS NewBookID;
END;
GO

-- Update Book
CREATE PROCEDURE sp_UpdateBook
    @BookID INT,
    @Title NVARCHAR(255),
	@ISBN NVARCHAR(20),
    @CategoryID INT,
	@AuthorID INT,
    @Edition NVARCHAR(50),
    @PublishedYear INT,
    @Language NVARCHAR(50),
	@Description NVARCHAR(MAX),
	@ShelfLocation NVARCHAR(50),
	@DailyRate DECIMAL(10, 2),
	@ImageName NVARCHAR(555)
AS
BEGIN
    UPDATE Books
    SET Title = @Title,
		ISBN = @ISBN,
        CategoryID = @CategoryID,
        AuthorID = @AuthorID,
		Edition = @Edition,
        PublishedYear = @PublishedYear,
        Language = @Language,
		Description = @Description,
        ShelfLocation = @ShelfLocation,
		DailyRate = @DailyRate,
        ImageName = @ImageName
    WHERE BookID = @BookID;
END;
GO

-- Delete Book
CREATE PROCEDURE sp_DeleteBook
    @BookID INT
AS
BEGIN
    DELETE FROM Books WHERE BookID = @BookID;
END;
GO

-- Is Book Exist
CREATE PROCEDURE sp_IsBookExist
    @BookID INT
AS
BEGIN
    IF EXISTS(SELECT * FROM Books WHERE BookID = @BookID)
        RETURN 1;  -- Book exists
    ELSE
        RETURN 0;  -- Book does not exist
END;
GO
