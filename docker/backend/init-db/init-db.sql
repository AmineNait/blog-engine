IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BlogDb')
BEGIN
    CREATE DATABASE [BlogDb];
END
GO

USE BlogDb;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categories')
BEGIN
    CREATE TABLE Categories (
        Id INT PRIMARY KEY IDENTITY,
        Name NVARCHAR(100) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Posts')
BEGIN
    CREATE TABLE Posts (
        Id INT PRIMARY KEY IDENTITY,
        Title NVARCHAR(200) NOT NULL,
        Content NVARCHAR(MAX) NOT NULL,
        CategoryId INT,
        FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
    );
END
GO
