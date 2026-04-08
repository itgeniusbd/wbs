-- =============================================
-- Complete Gallery Setup with Orphan Image Fix
-- Database: WBS_NGO
-- Description: Creates Galleries table and links orphan GalleryImages
-- =============================================

USE [WBS_NGO];
GO

PRINT '========================================';
PRINT 'Starting Gallery Setup...';
PRINT '========================================';
PRINT '';

-- ============================================
-- STEP 1: Create Galleries Table
-- ============================================

IF OBJECT_ID(N'[dbo].[Galleries]', N'U') IS NULL
BEGIN
    PRINT '?? Creating Galleries table...';
    
    CREATE TABLE [dbo].[Galleries] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Title] NVARCHAR(200) NOT NULL,
        [TitleBn] NVARCHAR(200) NULL,
        [Description] NVARCHAR(MAX) NULL,
        [DescriptionBn] NVARCHAR(MAX) NULL,
        [CoverImage] NVARCHAR(500) NULL,
        [DisplayOrder] INT NOT NULL CONSTRAINT DF_Galleries_DisplayOrder DEFAULT 0,
        [IsActive] BIT NOT NULL CONSTRAINT DF_Galleries_IsActive DEFAULT 1,
        [CreatedAt] DATETIME2 NOT NULL CONSTRAINT DF_Galleries_CreatedAt DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_Galleries] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    
    PRINT '? Galleries table created successfully!';
END
ELSE
BEGIN
    PRINT '? Galleries table already exists.';
END
GO

-- ============================================
-- STEP 2: Check for Orphan Images
-- ============================================

DECLARE @OrphanCount INT;

SELECT @OrphanCount = COUNT(*)
FROM [dbo].[GalleryImages]
WHERE [GalleryId] NOT IN (SELECT Id FROM [dbo].[Galleries]);

IF @OrphanCount > 0
BEGIN
    PRINT '';
    PRINT '??  Found ' + CAST(@OrphanCount AS NVARCHAR) + ' orphan images!';
    PRINT '?? Creating default gallery for orphan images...';
    
    -- Create default gallery
    INSERT INTO [dbo].[Galleries] 
        ([Title], [TitleBn], [Description], [DescriptionBn], [DisplayOrder], [IsActive], [CreatedAt])
    VALUES 
        ('General Photo Gallery', 
         '?????? ??? ????????',
         'Collection of photos from various events and activities',
         '??????? ???????? ??? ??????????? ???? ??????',
         1,
         1,
         GETUTCDATE());
    
    DECLARE @DefaultGalleryId INT = SCOPE_IDENTITY();
    
    -- Link orphan images to default gallery
    UPDATE [dbo].[GalleryImages]
    SET [GalleryId] = @DefaultGalleryId
    WHERE [GalleryId] NOT IN (SELECT Id FROM [dbo].[Galleries]);
    
    PRINT '? ' + CAST(@OrphanCount AS NVARCHAR) + ' orphan images linked to default gallery!';
END
ELSE
BEGIN
    PRINT '';
    PRINT '? No orphan images found.';
END
GO

-- ============================================
-- STEP 3: Add Foreign Key Constraint
-- ============================================

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys 
    WHERE name = 'FK_GalleryImages_Galleries'
)
BEGIN
    PRINT '';
    PRINT '?? Adding foreign key constraint...';
    
    ALTER TABLE [dbo].[GalleryImages]
    ADD CONSTRAINT [FK_GalleryImages_Galleries]
    FOREIGN KEY ([GalleryId]) REFERENCES [dbo].[Galleries]([Id])
    ON DELETE CASCADE;
    
    PRINT '? Foreign key constraint added successfully!';
END
ELSE
BEGIN
    PRINT '';
    PRINT '? Foreign key constraint already exists.';
END
GO

-- ============================================
-- STEP 4: Create Sample Galleries (Optional)
-- ============================================

-- Check if we need sample data
IF (SELECT COUNT(*) FROM [dbo].[Galleries]) < 3
BEGIN
    PRINT '';
    PRINT '?? Creating sample galleries...';
    
    -- Sample Gallery 1: Community Events
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Galleries] WHERE Title = 'Community Events 2024')
    BEGIN
        INSERT INTO [dbo].[Galleries] 
            ([Title], [TitleBn], [Description], [DescriptionBn], [DisplayOrder], [IsActive])
        VALUES 
            ('Community Events 2024',
             '?????????? ???????? ????',
             'Photos from our community development events and activities',
             '?????? ?????????? ??????? ???????? ??? ??????????? ???',
             2,
             1);
        PRINT '  ? Sample gallery 1 created';
    END
    
    -- Sample Gallery 2: Training Workshops
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Galleries] WHERE Title = 'Training Workshops')
    BEGIN
        INSERT INTO [dbo].[Galleries] 
            ([Title], [TitleBn], [Description], [DescriptionBn], [DisplayOrder], [IsActive])
        VALUES 
            ('Training Workshops',
             '????????? ????????',
             'Capacity building training sessions and workshops',
             '??????? ?????? ????????? ???? ??? ????????',
             3,
             1);
        PRINT '  ? Sample gallery 2 created';
    END
    
    -- Sample Gallery 3: Field Visits
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Galleries] WHERE Title = 'Field Visits')
    BEGIN
        INSERT INTO [dbo].[Galleries] 
            ([Title], [TitleBn], [Description], [DescriptionBn], [DisplayOrder], [IsActive])
        VALUES 
            ('Field Visits',
             '??? ????????',
             'Photos from field visits and project monitoring activities',
             '??? ???????? ??? ??????? ?????????? ??????????? ???',
             4,
             1);
        PRINT '  ? Sample gallery 3 created';
    END
END
GO

-- ============================================
-- STEP 5: Verify Setup
-- ============================================

PRINT '';
PRINT '========================================';
PRINT 'Verification:';
PRINT '========================================';

-- Check Galleries table
DECLARE @GalleriesCount INT;
SELECT @GalleriesCount = COUNT(*) FROM [dbo].[Galleries];
PRINT '? Galleries table: ' + CAST(@GalleriesCount AS NVARCHAR) + ' records';

-- Check GalleryImages table
DECLARE @ImagesCount INT;
SELECT @ImagesCount = COUNT(*) FROM [dbo].[GalleryImages];
PRINT '? GalleryImages table: ' + CAST(@ImagesCount AS NVARCHAR) + ' records';

-- Check orphan images
DECLARE @OrphanImagesCount INT;
SELECT @OrphanImagesCount = COUNT(*)
FROM [dbo].[GalleryImages]
WHERE [GalleryId] NOT IN (SELECT Id FROM [dbo].[Galleries]);
PRINT '? Orphan images: ' + CAST(@OrphanImagesCount AS NVARCHAR);

-- Check foreign key
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_GalleryImages_Galleries')
    PRINT '? Foreign key: Exists';
ELSE
    PRINT '? Foreign key: Missing';

PRINT '';
PRINT '========================================';
PRINT 'Gallery Setup Completed Successfully! ??';
PRINT '========================================';
PRINT '';
PRINT 'Next Steps:';
PRINT '1. Restart your application';
PRINT '2. Browse to: http://localhost:5001/activities/gallery';
PRINT '3. Admin Panel: http://localhost:5001/Admin/Galleries';
PRINT '';

-- Show summary
SELECT 
    g.Id,
    g.Title,
    g.TitleBn,
    g.IsActive,
    g.DisplayOrder,
    COUNT(gi.Id) as TotalImages,
    g.CreatedAt
FROM [dbo].[Galleries] g
LEFT JOIN [dbo].[GalleryImages] gi ON g.Id = gi.GalleryId
GROUP BY g.Id, g.Title, g.TitleBn, g.IsActive, g.DisplayOrder, g.CreatedAt
ORDER BY g.DisplayOrder;

GO
