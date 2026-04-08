-- SQL Script to Create CVApplications Table
-- Run this script in SQL Server Management Studio or using sqlcmd

USE [WBS_NGO];
GO

PRINT 'Creating CVApplications table...';
GO

-- Create CVApplications table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CVApplications')
BEGIN
    CREATE TABLE [dbo].[CVApplications](
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [FullName] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(100) NOT NULL,
        [Phone] NVARCHAR(20) NULL,
        [Address] NVARCHAR(200) NULL,
        [Education] NVARCHAR(200) NULL,
        [Experience] NVARCHAR(200) NULL,
        [Skills] NVARCHAR(MAX) NULL,
        [CVFilePath] NVARCHAR(MAX) NULL,
        [CoverLetter] NVARCHAR(MAX) NULL,
        [CareerIdAppliedFor] INT NULL,
        [PositionAppliedFor] NVARCHAR(200) NULL,
        [Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',
        [AppliedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [ReviewedDate] DATETIME2 NULL,
        [ReviewNotes] NVARCHAR(MAX) NULL
    );
    
    PRINT 'CVApplications table created successfully!';
END
ELSE
BEGIN
    PRINT 'CVApplications table already exists.';
END
GO

-- Create index on Email for faster searches
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CVApplications_Email')
BEGIN
    CREATE INDEX IX_CVApplications_Email ON [dbo].[CVApplications]([Email]);
    PRINT 'Index on Email created.';
END
GO

-- Create index on Status
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CVApplications_Status')
BEGIN
    CREATE INDEX IX_CVApplications_Status ON [dbo].[CVApplications]([Status]);
    PRINT 'Index on Status created.';
END
GO

-- Create index on AppliedDate
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CVApplications_AppliedDate')
BEGIN
    CREATE INDEX IX_CVApplications_AppliedDate ON [dbo].[CVApplications]([AppliedDate] DESC);
    PRINT 'Index on AppliedDate created.';
END
GO

PRINT 'CVApplications table setup completed successfully!';
GO

-- Display table structure
SELECT 
    COLUMN_NAME as 'Column Name',
    DATA_TYPE as 'Data Type',
    CHARACTER_MAXIMUM_LENGTH as 'Max Length',
    IS_NULLABLE as 'Nullable'
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'CVApplications'
ORDER BY ORDINAL_POSITION;
GO
