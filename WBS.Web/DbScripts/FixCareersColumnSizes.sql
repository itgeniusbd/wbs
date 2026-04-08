-- Fix Careers table column sizes for text fields
-- This script ensures all text columns can handle large content including rich text HTML

USE [WBS_NGO]
GO

PRINT 'Starting Careers table column size fix...';

-- Check if the columns exist and alter their data types
BEGIN TRY
    -- Fix Title columns
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'Title')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [Title] NVARCHAR(500) NOT NULL;
        PRINT 'Fixed Title column size (500 chars)';
    END

    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'TitleBn')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [TitleBn] NVARCHAR(500) NULL;
        PRINT 'Fixed TitleBn column size (500 chars)';
    END

    -- Fix Slug column
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'Slug')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [Slug] NVARCHAR(500) NOT NULL;
        PRINT 'Fixed Slug column size (500 chars)';
    END

    -- Fix Description
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'Description')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [Description] NVARCHAR(MAX) NULL;
        PRINT 'Fixed Description column size';
    END

    -- Fix DescriptionBn
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'DescriptionBn')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [DescriptionBn] NVARCHAR(MAX) NULL;
        PRINT 'Fixed DescriptionBn column size';
    END

    -- Fix Requirements
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'Requirements')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [Requirements] NVARCHAR(MAX) NULL;
        PRINT 'Fixed Requirements column size';
    END

    -- Fix RequirementsBn
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'RequirementsBn')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [RequirementsBn] NVARCHAR(MAX) NULL;
        PRINT 'Fixed RequirementsBn column size';
    END

    -- Fix Benefits
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'Benefits')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [Benefits] NVARCHAR(MAX) NULL;
        PRINT 'Fixed Benefits column size';
    END

    -- Fix Department
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'Department')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [Department] NVARCHAR(MAX) NULL;
        PRINT 'Fixed Department column size';
    END

    -- Fix Location
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'Location')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [Location] NVARCHAR(MAX) NULL;
        PRINT 'Fixed Location column size';
    END

    -- Fix JobType
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'JobType')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [JobType] NVARCHAR(MAX) NULL;
        PRINT 'Fixed JobType column size';
    END

    -- Fix SalaryRange
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'SalaryRange')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [SalaryRange] NVARCHAR(MAX) NULL;
        PRINT 'Fixed SalaryRange column size';
    END

    -- Fix ApplicationUrl
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'ApplicationUrl')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [ApplicationUrl] NVARCHAR(MAX) NULL;
        PRINT 'Fixed ApplicationUrl column size';
    END

    -- Fix ApplicationEmail
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'ApplicationEmail')
    BEGIN
        ALTER TABLE [dbo].[Careers] 
        ALTER COLUMN [ApplicationEmail] NVARCHAR(MAX) NULL;
        PRINT 'Fixed ApplicationEmail column size';
    END

    -- Fix UpdatedAt column (should be DATETIME2, not NCHAR)
    IF EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') 
               AND name = 'UpdatedAt')
    BEGIN
        -- Check if it's not already DATETIME2
        DECLARE @dataType NVARCHAR(50);
        SELECT @dataType = DATA_TYPE 
        FROM INFORMATION_SCHEMA.COLUMNS 
        WHERE TABLE_NAME = 'Careers' AND COLUMN_NAME = 'UpdatedAt';
        
        IF @dataType != 'datetime2'
        BEGIN
            ALTER TABLE [dbo].[Careers] 
            ALTER COLUMN [UpdatedAt] DATETIME2 NULL;
            PRINT 'Fixed UpdatedAt column type to DATETIME2';
        END
    END

    PRINT '';
    PRINT '================================================';
    PRINT 'All Careers table columns fixed successfully!';
    PRINT '================================================';
    PRINT '';
    PRINT 'Column sizes:';
    PRINT '- Title, TitleBn, Slug: NVARCHAR(500)';
    PRINT '- Text fields (Description, Requirements, etc.): NVARCHAR(MAX)';
    PRINT '- UpdatedAt: DATETIME2';
    PRINT '';
    PRINT 'You can now save large content including HTML from TinyMCE editor.';

END TRY
BEGIN CATCH
    PRINT 'Error occurred:';
    PRINT ERROR_MESSAGE();
END CATCH
GO

-- Verification Query
PRINT '';
PRINT 'Verification of column types:';
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CASE 
        WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
        ELSE CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR)
    END AS MAX_LENGTH,
    IS_NULLABLE
FROM 
    INFORMATION_SCHEMA.COLUMNS
WHERE 
    TABLE_NAME = 'Careers'
ORDER BY 
    ORDINAL_POSITION;
