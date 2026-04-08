-- =============================================
-- Complete Gallery Fix - Handle VideoGalleries Migration
-- =============================================

USE [WBS_NGO];
GO

PRINT '========================================';
PRINT 'Starting Complete Gallery Fix...';
PRINT '========================================';
PRINT '';

-- ============================================
-- STEP 1: Check Existing Tables
-- ============================================

PRINT 'STEP 1: Checking existing tables...';
PRINT '';

-- Check VideoGalleries
DECLARE @HasVideoGalleries BIT = 0;
DECLARE @VideoGalleriesCount INT = 0;

IF OBJECT_ID(N'[dbo].[VideoGalleries]', N'U') IS NOT NULL
BEGIN
    SET @HasVideoGalleries = 1;
    SELECT @VideoGalleriesCount = COUNT(*) FROM [dbo].[VideoGalleries];
    PRINT '??  VideoGalleries table exists with ' + CAST(@VideoGalleriesCount AS NVARCHAR) + ' records';
END
ELSE
BEGIN
    PRINT '? No VideoGalleries table found';
END

-- Check Galleries
DECLARE @GalleriesCount INT = 0;
SELECT @GalleriesCount = COUNT(*) FROM [dbo].[Galleries];
PRINT '? Galleries table has ' + CAST(@GalleriesCount AS NVARCHAR) + ' records';

-- Check GalleryImages
DECLARE @GalleryImagesCount INT = 0;
SELECT @GalleryImagesCount = COUNT(*) FROM [dbo].[GalleryImages];
PRINT '? GalleryImages table has ' + CAST(@GalleryImagesCount AS NVARCHAR) + ' records';

PRINT '';

-- ============================================
-- STEP 2: Drop Conflicting Foreign Keys
-- ============================================

PRINT 'STEP 2: Removing conflicting foreign keys...';
PRINT '';

-- Drop all foreign keys from GalleryImages
DECLARE @FKName NVARCHAR(255);
DECLARE fk_cursor CURSOR FOR
SELECT name 
FROM sys.foreign_keys 
WHERE parent_object_id = OBJECT_ID('GalleryImages');

OPEN fk_cursor;
FETCH NEXT FROM fk_cursor INTO @FKName;

WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE @DropSQL NVARCHAR(MAX) = 'ALTER TABLE [dbo].[GalleryImages] DROP CONSTRAINT [' + @FKName + '];';
    PRINT '   Dropping: ' + @FKName;
    EXEC sp_executesql @DropSQL;
    FETCH NEXT FROM fk_cursor INTO @FKName;
END

CLOSE fk_cursor;
DEALLOCATE fk_cursor;

PRINT '? All old foreign keys dropped';
PRINT '';

-- ============================================
-- STEP 3: Create Correct Foreign Key
-- ============================================

PRINT 'STEP 3: Creating correct foreign key...';
PRINT '';

ALTER TABLE [dbo].[GalleryImages]
ADD CONSTRAINT [FK_GalleryImages_Galleries_GalleryId]
FOREIGN KEY ([GalleryId]) REFERENCES [dbo].[Galleries]([Id])
ON DELETE CASCADE;

PRINT '? New foreign key created successfully!';
PRINT '';

-- ============================================
-- STEP 4: Verify All Gallery Images Have Valid GalleryId
-- ============================================

PRINT 'STEP 4: Checking data integrity...';
PRINT '';

-- Check for orphan images
DECLARE @OrphanImages INT;
SELECT @OrphanImages = COUNT(*)
FROM [dbo].[GalleryImages] gi
LEFT JOIN [dbo].[Galleries] g ON gi.GalleryId = g.Id
WHERE g.Id IS NULL;

IF @OrphanImages > 0
BEGIN
    PRINT '??  Found ' + CAST(@OrphanImages AS NVARCHAR) + ' orphan images!';
    PRINT '   Creating default gallery for orphan images...';
    
    -- Check if default gallery exists
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Galleries] WHERE Title = 'Uncategorized Gallery')
    BEGIN
        INSERT INTO [dbo].[Galleries] 
            ([Title], [TitleBn], [Description], [DescriptionBn], [DisplayOrder], [IsActive], [CreatedAt])
        VALUES 
            ('Uncategorized Gallery', 
             '???????????? ????????',
             'Images that were not assigned to any gallery',
             '?? ??????? ???? ?????????? ?????? ??? ?????',
             999,
             1,
             GETUTCDATE());
        
        PRINT '   ? Default gallery created';
    END
    
    DECLARE @DefaultGalleryId INT;
    SELECT @DefaultGalleryId = Id FROM [dbo].[Galleries] WHERE Title = 'Uncategorized Gallery';
    
    -- Update orphan images
    UPDATE gi
    SET gi.GalleryId = @DefaultGalleryId
    FROM [dbo].[GalleryImages] gi
    LEFT JOIN [dbo].[Galleries] g ON gi.GalleryId = g.Id
    WHERE g.Id IS NULL;
    
    PRINT '   ? Orphan images linked to default gallery';
END
ELSE
BEGIN
    PRINT '? No orphan images found. All images have valid GalleryId.';
END

PRINT '';

-- ============================================
-- STEP 5: Optional - Handle VideoGalleries
-- ============================================

IF @HasVideoGalleries = 1 AND @VideoGalleriesCount > 0
BEGIN
    PRINT 'STEP 5: VideoGalleries table found with data';
    PRINT '';
    PRINT '??  NOTE: VideoGalleries table exists but is not being used.';
    PRINT '   If you want to migrate data from VideoGalleries to Galleries,';
    PRINT '   please run a separate migration script.';
    PRINT '';
    PRINT '   For now, we will keep VideoGalleries as is.';
    PRINT '';
END

-- ============================================
-- STEP 6: Final Verification
-- ============================================

PRINT '========================================';
PRINT 'Final Verification:';
PRINT '========================================';
PRINT '';

-- Show foreign keys
PRINT 'Foreign Keys:';
SELECT 
    '? ' + fk.name AS Status,
    OBJECT_NAME(fk.parent_object_id) AS [Table],
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS [Column],
    OBJECT_NAME(fk.referenced_object_id) AS [References],
    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS [Referenced Column]
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fc ON fk.object_id = fc.constraint_object_id
WHERE fk.parent_object_id = OBJECT_ID('GalleryImages');

PRINT '';

-- Show data summary
PRINT 'Data Summary:';
SELECT 
    'Galleries' AS TableName,
    COUNT(*) AS RecordCount,
    SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END) AS ActiveCount
FROM [dbo].[Galleries]
UNION ALL
SELECT 
    'GalleryImages' AS TableName,
    COUNT(*) AS RecordCount,
    SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END) AS ActiveCount
FROM [dbo].[GalleryImages];

PRINT '';
PRINT '========================================';
PRINT '? Gallery Fix Completed Successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Next Steps:';
PRINT '1. Restart your application';
PRINT '2. Go to Admin Panel -> Galleries';
PRINT '3. Try uploading images';
PRINT '';
PRINT 'If you still get errors, please check:';
PRINT '- Application connection string';
PRINT '- Image file sizes (max 2MB recommended)';
PRINT '- Upload folder permissions';
PRINT '';
GO
