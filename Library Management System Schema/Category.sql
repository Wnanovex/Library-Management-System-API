
-- Get Category By ID
CREATE PROCEDURE sp_GetCategoryByID
    @CategoryID INT
AS
BEGIN
    SELECT * FROM Categories WHERE CategoryID = @CategoryID;
END;
GO

-- Get Category By Name
CREATE PROCEDURE sp_GetCategoryByName
    @Name NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Categories WHERE Name = @Name;
END;
GO


-- Get All Categories
CREATE PROCEDURE sp_GetAllCategories
AS
BEGIN
    SELECT * FROM Categories ORDER BY CategoryID;
END;
GO

-- Add New Category
CREATE PROCEDURE sp_AddNewCategory
    @Name NVARCHAR(100),
    @Description NVARCHAR(250)
AS
BEGIN
    INSERT INTO Categories (Name, Description)
    VALUES (@Name, @Description);
    
    -- Return newly created CategoryID
    SELECT SCOPE_IDENTITY() AS NewCategoryID;
END;
GO

-- Update Category
CREATE PROCEDURE sp_UpdateCategory
    @CategoryID INT,
    @Name NVARCHAR(100),
    @Description NVARCHAR(250)
AS
BEGIN
    UPDATE Categories
    SET Name = @Name,
		Description = @Description
    WHERE CategoryID = @CategoryID;
END;
GO

-- Delete Category
CREATE PROCEDURE sp_DeleteCategory
    @CategoryID INT
AS
BEGIN
    DELETE FROM Categories WHERE CategoryID = @CategoryID;
END;
GO
