
-- Get IssuedBook By ID
CREATE PROCEDURE sp_GetIssuedBookByID
    @IssueID INT
AS
BEGIN
    SELECT * FROM IssuedBooks WHERE IssueID = @IssueID;
END;
GO

-- Get All IssuedBooks
CREATE PROCEDURE sp_GetAllIssuedBooks
AS
BEGIN
    SELECT * FROM IssuedBooks;
END;
GO

-- Add New IssuedBook
CREATE PROCEDURE sp_AddNewIssuedBook
    @CopyID INT,
	@MemberID INT,
    @IssuedBy INT,
    @DueDate DATE,
    @IsReturned BIT = 0
AS
BEGIN
    INSERT INTO IssuedBooks (CopyID, MemberID, IssuedBy, IssueDate, DueDate, IsReturned)
    VALUES (@CopyID, @MemberID, @IssuedBy, GETDATE(), @DueDate, @IsReturned);
    
    -- Return newly created IssueID
    SELECT SCOPE_IDENTITY() AS NewIssueID;
END;
GO

-- Update IssuedBook
CREATE PROCEDURE sp_UpdateIssuedBook
    @IssueID INT,
    @CopyID INT,
	@MemberID INT,
    @DueDate DATE,
    @ReturnDate DATE,
    @IsReturned BIT
AS
BEGIN
    UPDATE IssuedBooks
    SET CopyID = @CopyID,
		MemberID = @MemberID,
        DueDate = @DueDate,
        ReturnDate = @ReturnDate,
		IsReturned = @IsReturned
    WHERE IssueID = @IssueID;
END;
GO

CREATE PROCEDURE [dbo].[sp_MarkBookAsReturned]
    @IssueID INT,
    @ReturnDate DATE = NULL
AS
BEGIN

    UPDATE IssuedBooks
    SET 
        IsReturned = 1,
        ReturnDate = ISNULL(@ReturnDate, GETDATE())
    WHERE IssueID = @IssueID;
END;
Go

-- Delete IssuedBook
CREATE PROCEDURE sp_DeleteIssuedBook
    @IssueID INT
AS
BEGIN
    DELETE FROM IssuedBooks WHERE IssueID = @IssueID;
END;
GO

-- Is Book Returned
CREATE PROCEDURE sp_IsBookReturned
    @IssueID INT
AS
BEGIN
    IF EXISTS(SELECT * FROM IssuedBooks WHERE IssueID = @IssueID AND IsReturned = 1)
        RETURN 1;
    ELSE
        RETURN 0; 
END;
GO
