
CREATE PROCEDURE sp_GetDashboardCounts
AS
BEGIN
    SELECT 
        (SELECT COUNT(*) FROM Books) AS TotalBooks,
		(SELECT COUNT(*) FROM BookCopies) AS TotalBookCopies,
        (SELECT COUNT(*) FROM Members) AS TotalMembers,
        (SELECT COUNT(*) FROM IssuedBooks) AS TotalIssuedBooks;
END
GO

CREATE PROCEDURE sp_GetDailyDashboardCounts
AS
BEGIN
    SELECT
        (SELECT COUNT(*) FROM Books WHERE CAST(DateAdded AS DATE) = CAST(GETDATE() AS DATE)) AS TotalBooksToday,
        (SELECT COUNT(*) FROM People WHERE CAST(CreatedAt AS DATE) = CAST(GETDATE() AS DATE)) AS TotalMembersToday,
        (SELECT COUNT(*) FROM IssuedBooks WHERE CAST(IssueDate AS DATE) = CAST(GETDATE() AS DATE)) AS TotalIssuedToday
END