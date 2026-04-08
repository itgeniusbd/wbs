-- =============================================
-- Check if Galleries table exists
-- =============================================

USE [WBS_NGO];
GO

-- Check if Galleries table exists
IF OBJECT_ID(N'[dbo].[Galleries]', N'U') IS NULL
BEGIN
    PRINT '? Galleries table does NOT exist!';
    PRINT 'Creating Galleries table...';
    
    -- Create Galleries table
    CREATE TABLE [dbo].[Galleries] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Title] NVARCHAR(200) NOT NULL,
        [TitleBn] NVARCHAR(200) NULL,
        [Description] NVARCHAR(MAX) NULL,
        [DescriptionBn] NVARCHAR(MAX) NULL,
        [CoverImage] NVARCHAR(500) NULL,
        [DisplayOrder] INT NOT NULL DEFAULT 0,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
    
    PRINT '? Galleries table created successfully!';
END
ELSE
BEGIN
    PRINT '? Galleries table already exists.';
END
GO

-- Check if foreign key exists
IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys 
    WHERE name = 'FK_GalleryImages_Galleries'
)
BEGIN
    PRINT 'Adding foreign key constraint...';
    
    ALTER TABLE [dbo].[GalleryImages]
    ADD CONSTRAINT [FK_GalleryImages_Galleries]
    FOREIGN KEY ([GalleryId]) REFERENCES [dbo].[Galleries]([Id])
    ON DELETE CASCADE;
    
    PRINT '? Foreign key added successfully!';
END
ELSE
BEGIN
    PRINT '? Foreign key already exists.';
END
GO

-- Verify table structure
PRINT '';
PRINT '========================================';
PRINT 'Galleries Table Structure:';
PRINT '========================================';
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Galleries'
ORDER BY ORDINAL_POSITION;
GO

PRINT '';
PRINT '========================================';
PRINT 'GalleryImages Table Structure:';
PRINT '========================================';
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'GalleryImages'
ORDER BY ORDINAL_POSITION;
GO

-- Check existing data
PRINT '';
PRINT '========================================';
PRINT 'Data Summary:';
PRINT '========================================';
SELECT 
    'Galleries' as TableName,
    COUNT(*) as RecordCount
FROM [dbo].[Galleries]
UNION ALL
SELECT 
    'GalleryImages' as TableName,
    COUNT(*) as RecordCount
FROM [dbo].[GalleryImages];
GO

PRINT '';
PRINT '? Migration completed successfully!';
GO
