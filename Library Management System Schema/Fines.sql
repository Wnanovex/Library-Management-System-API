
-- Get Fine By ID
CREATE PROCEDURE sp_GetFineByID
    @FineID INT
AS
BEGIN
    SELECT * FROM Fines WHERE FineID = @FineID;
END;
GO

CREATE PROCEDURE sp_GetFineByIssueID
    @IssueID INT
AS
BEGIN
    SELECT * FROM Fines WHERE IssueID = @IssueID;
END
Go

-- Get All Fines
CREATE PROCEDURE sp_GetAllFines
AS
BEGIN
    SELECT * FROM Fines;
END;
GO

-- Add New Fine
CREATE PROCEDURE sp_AddNewFine
    @IssueID INT
AS
BEGIN
    DECLARE @DueDate DATE,
            @ReturnDate DATE,
            @DailyRate DECIMAL(10,2),
            @Amount DECIMAL(10,2);

    -- Get DueDate, ReturnDate, DailyRate
    SELECT @DueDate = IssuedBooks.DueDate,
           @ReturnDate = IssuedBooks.ReturnDate,
           @DailyRate = Books.DailyRate
    FROM IssuedBooks
    INNER JOIN BookCopies ON IssuedBooks.CopyID = BookCopies.CopyID
    INNER JOIN Books ON BookCopies.BookID = Books.BookID
    WHERE IssueID = @IssueID;

    -- ✅ Validate required data exists
    IF @DueDate IS NULL OR @ReturnDate IS NULL OR @DailyRate IS NULL
    BEGIN
        -- Return NULL explicitly and EXIT
        SELECT CAST(NULL AS INT) AS NewFineID;
        RETURN;
    END

    -- ✅ Calculate fine
    SET @Amount = dbo.ufn_CalculateFine(@DueDate, @ReturnDate, @DailyRate);

    IF @Amount > 0
    BEGIN
        INSERT INTO Fines (IssueID, Amount, Paid, DatePaid)
        VALUES (@IssueID, @Amount, 0, NULL);

        -- ✅ Return the new FineID
        SELECT SCOPE_IDENTITY() AS NewFineID;
    END
    ELSE
    BEGIN
        -- ✅ Return NULL if no fine needed
        SELECT CAST(NULL AS INT) AS NewFineID;
    END
END
GO

---- Update Fine
--CREATE PROCEDURE sp_UpdateFine
--    @FineID INT,
--	@Amount DECIMAL(10,2),
--    @Paid BIT = 0,
--    @DatePaid DATE NULL
--AS
--BEGIN
--    UPDATE Fines
--    SET Amount = @Amount,
--        Paid = @Paid,
--        DatePaid = @DatePaid
--    WHERE FineID = @FineID;
--END;
--GO

-- Delete Fine
CREATE PROCEDURE sp_DeleteFine
    @FineID INT
AS
BEGIN
    DELETE FROM Fines WHERE FineID = @FineID;
END;
GO

CREATE PROCEDURE sp_PayFine
    @FineID INT
AS
BEGIN
    UPDATE Fines
    SET Paid = 1,
        DatePaid = GETDATE()
    WHERE FineID = @FineID;
END;
Go


CREATE FUNCTION ufn_CalculateFine (
    @DueDate DATE,
    @ReturnDate DATE,
    @DailyRate DECIMAL(10,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @Fine DECIMAL(10,2);

    IF (@ReturnDate <= @DueDate)
        SET @Fine = 0;
    ELSE
        SET @Fine = DATEDIFF(DAY, @DueDate, @ReturnDate) * @DailyRate;

    RETURN @Fine;
END
GO