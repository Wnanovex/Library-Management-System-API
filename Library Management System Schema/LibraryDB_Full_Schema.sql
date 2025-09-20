-- ===============================================
-- DATABASE SCHEMA: LibraryDB
-- ===============================================

-- 1. Create Database
IF DB_ID('LibraryDB') IS NULL
BEGIN
    CREATE DATABASE LibraryDB;
END
GO

USE LibraryDB;
GO

-- ===============================================
-- 2. People Table (base table)
-- ===============================================
CREATE TABLE People (
    PersonID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender CHAR(1) CHECK (Gender IN ('M', 'F')) NOT NULL,
    Email NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NOT NULL,
    Address NVARCHAR(150) NOT NULL,
    City NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);
GO

-- ===============================================
-- 3. Users Table
-- ===============================================
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    PersonID INT NULL,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Role SMALLINT DEFAULT 1 NOT NULL,
    LastLogin DATETIME NULL,
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (PersonID) REFERENCES People(PersonID) ON DELETE CASCADE
);
GO

-- ===============================================
-- 4. Members Table
-- ===============================================
CREATE TABLE Members (
    MemberID INT PRIMARY KEY IDENTITY(1,1),
    PersonID INT NULL,
    DateJoined DATE DEFAULT GETDATE(),
    FOREIGN KEY (PersonID) REFERENCES People(PersonID) ON DELETE CASCADE
);
GO

-- ===============================================
-- 5. Authors Table
-- ===============================================
CREATE TABLE Authors (
    AuthorID INT PRIMARY KEY IDENTITY(1,1),
    PersonID INT NULL,
    Biography NVARCHAR(MAX),
    FOREIGN KEY (PersonID) REFERENCES People(PersonID) ON DELETE CASCADE
);
GO

-- ===============================================
-- 6. Categories Table
-- ===============================================
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(250)
);
GO

-- ===============================================
-- 7. Books Table
-- ===============================================
CREATE TABLE Books (
    BookID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(150) NOT NULL,
    ISBN NVARCHAR(20) UNIQUE NOT NULL,
    CategoryID INT NULL,
    AuthorID INT NULL,
    Edition NVARCHAR(50),
    PublishedYear INT CHECK (PublishedYear >= 0),
    Language NVARCHAR(50),
    Description NVARCHAR(MAX),
    TotalCopies INT NULL CHECK (TotalCopies >= 0),
    ShelfLocation NVARCHAR(50),
	DailyRate DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
	ImageName NVARCHAR(555) NULL,
    DateAdded DATE DEFAULT GETDATE(),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID) ON DELETE SET NULL,
    FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID) ON DELETE SET NULL
);
GO

-- ===============================================
-- 8. BookCopies Table
-- ===============================================
CREATE TABLE BookCopies (
    CopyID INT PRIMARY KEY IDENTITY(1,1),
    BookID INT NOT NULL,
    Status SMALLINT DEFAULT 1,
    DateAdded DATE DEFAULT GETDATE(),
    FOREIGN KEY (BookID) REFERENCES Books(BookID) ON DELETE CASCADE,
);
GO

-- ===============================================
-- 9. IssuedBooks Table
-- ===============================================
CREATE TABLE IssuedBooks (
    IssueID INT PRIMARY KEY IDENTITY(1,1),
    CopyID INT NOT NULL,
    MemberID INT NOT NULL,
    IssuedBy INT NULL,
    IssueDate DATE NOT NULL DEFAULT GETDATE(),
    DueDate DATE NOT NULL,
    ReturnDate DATE NULL,
    IsReturned BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (CopyID) REFERENCES BookCopies(CopyID) ON DELETE NO ACTION,
    FOREIGN KEY (MemberID) REFERENCES Members(MemberID) ON DELETE NO ACTION,
    FOREIGN KEY (IssuedBy) REFERENCES Users(UserID) ON DELETE SET NULL
);
GO

-- ===============================================
-- 10. Fines Table
-- ===============================================
CREATE TABLE Fines (
    FineID INT PRIMARY KEY IDENTITY(1,1),
    IssueID INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL CHECK (Amount >= 0),
    Paid BIT NOT NULL DEFAULT 0,
    DatePaid DATE NULL,
    FOREIGN KEY (IssueID) REFERENCES IssuedBooks(IssueID) ON DELETE NO ACTION
);
GO

-- ===============================================
-- 11. AuditLogs Table
-- ===============================================
CREATE TABLE AuditLogs (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NULL,
    Action NVARCHAR(200) NOT NULL,
    TableAffected NVARCHAR(100),
    Timestamp DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE SET NULL
);
GO
