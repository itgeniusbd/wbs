-- =============================================
-- Fix Gallery Foreign Key Conflict
-- Issue: Conflict with VideoGalleries table
-- =============================================

USE [WBS_NGO];
GO

PRINT '========================================';
PRINT 'Checking Gallery Tables Structure...';
PRINT '========================================';
PRINT '';

-- Check if VideoGalleries table exists
IF OBJECT_ID(N'[dbo].[VideoGalleries]', N'U') IS NOT NULL
BEGIN
    PRINT '??  Found VideoGalleries table (old table)';
    
    -- Check if it has any data
    DECLARE @VideoGalleriesCount INT;
    SELECT @VideoGalleriesCount = COUNT(*) FROM [dbo].[VideoGalleries];
    PRINT '   Records in VideoGalleries: ' + CAST(@VideoGalleriesCount AS NVARCHAR);
END
ELSE
BEGIN
    PRINT '? VideoGalleries table does not exist.';
END
GO

-- Check existing foreign key
IF EXISTS (
    SELECT 1 FROM sys.foreign_keys 
    WHERE name = 'FK_GalleryImages_Galleries'
)
BEGIN
    PRINT '';
    PRINT '??  Existing foreign key found: FK_GalleryImages_Galleries';
    PRINT '   Dropping old foreign key...';
    
    ALTER TABLE [dbo].[GalleryImages]
    DROP CONSTRAINT [FK_GalleryImages_Galleries];
    
    PRINT '? Old foreign key dropped.';
END
GO

-- Create correct foreign key
PRINT '';
PRINT '?? Creating new foreign key...';

ALTER TABLE [dbo].[GalleryImages]
ADD CONSTRAINT [FK_GalleryImages_Galleries_GalleryId]
FOREIGN KEY ([GalleryId]) REFERENCES [dbo].[Galleries]([Id])
ON DELETE CASCADE;

PRINT '? New foreign key created: FK_GalleryImages_Galleries_GalleryId';
GO

-- Verify the fix
PRINT '';
PRINT '========================================';
PRINT 'Verification:';
PRINT '========================================';

-- Check foreign keys
SELECT 
    fk.name AS ForeignKeyName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable,
    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferencedColumn
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fc ON fk.object_id = fc.constraint_object_id
WHERE fk.parent_object_id = OBJECT_ID('GalleryImages');
GO

PRINT '';
PRINT '========================================';
PRINT '? Foreign Key Fix Completed!';
PRINT '========================================';
PRINT '';
PRINT 'Next Steps:';
PRINT '1. Restart your application';
PRINT '2. Try uploading images again';
PRINT '';
GO
