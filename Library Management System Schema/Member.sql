
-- Get Member By ID
CREATE PROCEDURE sp_GetMemberByID
    @MemberID INT
AS
BEGIN
    SELECT * FROM Members WHERE MemberID = @MemberID;
END;
GO

-- Get Member By PersonID
CREATE PROCEDURE sp_GetMemberByPersonID
    @PersonID INT
AS
BEGIN
    SELECT * FROM Members WHERE PersonID = @PersonID;
END;
GO

-- Get All Members
CREATE PROCEDURE sp_GetAllMembers
AS
BEGIN
    SELECT  Members.MemberID, People.FirstName, People.LastName, People.DateOfBirth, GenderCaption=CASE WHEN People.Gender='M' THEN 'Male' WHEN People.Gender='F' THEN 'Female' END, People.Email, People.Phone, People.Address, People.City, Members.DateJoined
		FROM      People INNER JOIN
                 Members ON People.PersonID = Members.PersonID ORDER BY MemberID;
END;
GO

-- Get All Members2
CREATE PROCEDURE sp_GetAllMembers2
AS
BEGIN
    SELECT * from Members ORDER BY MemberID;
END;
GO

-- Add New Member
CREATE PROCEDURE sp_AddNewMember
    @PersonID INT
AS
BEGIN
    INSERT INTO Members (PersonID, DateJoined)
    VALUES (@PersonID, GETDATE());
    
    -- Return newly created MemberID
    SELECT SCOPE_IDENTITY() AS NewMemberID;
END;
GO

-- Update Member
--CREATE PROCEDURE sp_UpdateMember
    
--AS
--BEGIN
--    UPDATE Members
--    SET 
--    WHERE MemberID = @MemberID;
--END;
--GO

-- Delete Member
CREATE PROCEDURE sp_DeleteMember
    @MemberID INT
AS
BEGIN
    DELETE FROM Members WHERE MemberID = @MemberID;
END;
GO

-- Is Member Exist
CREATE PROCEDURE sp_IsMemberExist
    @MemberID INT
AS
BEGIN
    IF EXISTS(SELECT * FROM Members WHERE MemberID = @MemberID)
        RETURN 1;  -- Member exists
    ELSE
        RETURN 0;  -- Member does not exist
END;
GO
