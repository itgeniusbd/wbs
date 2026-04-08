-- SQL Script to Add Missing Columns to Existing Careers Table
-- This will NOT delete your existing data
-- Run this script in SQL Server Management Studio

USE [WBS_NGO];
GO

PRINT 'Starting to add missing columns to Careers table...';
GO

-- Add Slug column (required)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'Slug')
BEGIN
    ALTER TABLE [dbo].[Careers] ADD [Slug] NVARCHAR(300) NOT NULL DEFAULT '';
    PRINT 'Added Slug column';
    
    -- Update Slug for existing records based on Title
    UPDATE [dbo].[Careers]
    SET [Slug] = LOWER(REPLACE(REPLACE(REPLACE(REPLACE(Title, ' ', '-'), '''', ''), ',', ''), '.', ''))
    WHERE [Slug] = '' OR [Slug] IS NULL;
    PRINT 'Updated Slug values for existing records';
END
ELSE
BEGIN
    PRINT 'Slug column already exists';
END
GO

-- Add Department column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'Department')
BEGIN
    ALTER TABLE [dbo].[Careers] ADD [Department] NVARCHAR(MAX) NULL;
    PRINT 'Added Department column';
END
ELSE
BEGIN
    PRINT 'Department column already exists';
END
GO

-- Add Requirements column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'Requirements')
BEGIN
    ALTER TABLE [dbo].[Careers] ADD [Requirements] NVARCHAR(MAX) NULL;
    PRINT 'Added Requirements column';
END
ELSE
BEGIN
    PRINT 'Requirements column already exists';
END
GO

-- Add RequirementsBn column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'RequirementsBn')
BEGIN
    ALTER TABLE [dbo].[Careers] ADD [RequirementsBn] NVARCHAR(MAX) NULL;
    PRINT 'Added RequirementsBn column';
END
ELSE
BEGIN
    PRINT 'RequirementsBn column already exists';
END
GO

-- Add Benefits column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'Benefits')
BEGIN
    ALTER TABLE [dbo].[Careers] ADD [Benefits] NVARCHAR(MAX) NULL;
    PRINT 'Added Benefits column';
END
ELSE
BEGIN
    PRINT 'Benefits column already exists';
END
GO

-- Add SalaryRange column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'SalaryRange')
BEGIN
    ALTER TABLE [dbo].[Careers] ADD [SalaryRange] NVARCHAR(MAX) NULL;
    PRINT 'Added SalaryRange column';
END
ELSE
BEGIN
    PRINT 'SalaryRange column already exists';
END
GO

-- Add ApplicationUrl column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'ApplicationUrl')
BEGIN
    ALTER TABLE [dbo].[Careers] ADD [ApplicationUrl] NVARCHAR(MAX) NULL;
    PRINT 'Added ApplicationUrl column';
END
ELSE
BEGIN
    PRINT 'ApplicationUrl column already exists';
END
GO

-- Add ApplicationEmail column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'ApplicationEmail')
BEGIN
    ALTER TABLE [dbo].[Careers] ADD [ApplicationEmail] NVARCHAR(MAX) NULL;
    PRINT 'Added ApplicationEmail column';
    
    -- Set default email for existing records
    UPDATE [dbo].[Careers]
    SET [ApplicationEmail] = 'careers@wbs.org'
    WHERE [ApplicationEmail] IS NULL;
    PRINT 'Set default email for existing records';
END
ELSE
BEGIN
    PRINT 'ApplicationEmail column already exists';
END
GO

-- Add CreatedAt column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'CreatedAt')
BEGIN
    ALTER TABLE [dbo].[Careers] ADD [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE();
    PRINT 'Added CreatedAt column';
    
    -- Update CreatedAt from PostedDate if PostedDate exists
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'PostedDate')
    BEGIN
        UPDATE [dbo].[Careers]
        SET [CreatedAt] = [PostedDate]
        WHERE [PostedDate] IS NOT NULL;
        PRINT 'Updated CreatedAt from PostedDate';
    END
END
ELSE
BEGIN
    PRINT 'CreatedAt column already exists';
END
GO

-- Add UpdatedAt column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'UpdatedAt')
BEGIN
    ALTER TABLE [dbo].[Careers] ADD [UpdatedAt] DATETIME2 NULL;
    PRINT 'Added UpdatedAt column';
END
ELSE
BEGIN
    PRINT 'UpdatedAt column already exists';
END
GO

PRINT '';
PRINT '=================================================================';
PRINT 'Migration completed successfully!';
PRINT 'All missing columns have been added to the Careers table.';
PRINT 'Your existing data is safe and preserved.';
PRINT '=================================================================';
PRINT '';

-- Display the updated table structure
PRINT 'Current Careers table structure:';
SELECT 
    COLUMN_NAME as 'Column Name',
    DATA_TYPE as 'Data Type',
    CHARACTER_MAXIMUM_LENGTH as 'Max Length',
    IS_NULLABLE as 'Nullable'
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Careers'
ORDER BY ORDINAL_POSITION;
GO

-- Display existing records count
PRINT '';
PRINT 'Total career records: ' + CAST((SELECT COUNT(*) FROM [dbo].[Careers]) AS VARCHAR(10));
GO

PRINT '';
PRINT 'You can now run your application. The Career page should work!';
GO
