-- =============================================
-- Script: Update Gallery Tables - Add Missing Columns
-- Description: Adds IsActive, CreatedAt, and DisplayOrder columns to Gallery and GalleryImages tables
-- Date: 2024
-- =============================================

USE [WBS_NGO];
GO

PRINT 'Starting Gallery tables migration...';
GO

-- ============================================
-- Update Galleries Table
-- ============================================

-- Add IsActive column to Galleries
IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[Galleries]') 
    AND name = 'IsActive'
)
BEGIN
    ALTER TABLE [dbo].[Galleries]
    ADD [IsActive] BIT NOT NULL DEFAULT 1;
    PRINT 'IsActive column added to Galleries table.';
END
ELSE
BEGIN
    PRINT 'IsActive column already exists in Galleries table.';
END
GO

-- Add CreatedAt column to Galleries
IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[Galleries]') 
    AND name = 'CreatedAt'
)
BEGIN
    ALTER TABLE [dbo].[Galleries]
    ADD [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE();
    PRINT 'CreatedAt column added to Galleries table.';
END
ELSE
BEGIN
    PRINT 'CreatedAt column already exists in Galleries table.';
END
GO

-- Add DisplayOrder column to Galleries
IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[Galleries]') 
    AND name = 'DisplayOrder'
)
BEGIN
    ALTER TABLE [dbo].[Galleries]
    ADD [DisplayOrder] INT NOT NULL DEFAULT 0;
    PRINT 'DisplayOrder column added to Galleries table.';
END
ELSE
BEGIN
    PRINT 'DisplayOrder column already exists in Galleries table.';
END
GO

-- ============================================
-- Update GalleryImages Table
-- ============================================

-- Add IsActive column to GalleryImages
IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[GalleryImages]') 
    AND name = 'IsActive'
)
BEGIN
    ALTER TABLE [dbo].[GalleryImages]
    ADD [IsActive] BIT NOT NULL DEFAULT 1;
    PRINT 'IsActive column added to GalleryImages table.';
END
ELSE
BEGIN
    PRINT 'IsActive column already exists in GalleryImages table.';
END
GO

-- Add CreatedAt column to GalleryImages
IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[GalleryImages]') 
    AND name = 'CreatedAt'
)
BEGIN
    ALTER TABLE [dbo].[GalleryImages]
    ADD [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE();
    PRINT 'CreatedAt column added to GalleryImages table.';
END
ELSE
BEGIN
    PRINT 'CreatedAt column already exists in GalleryImages table.';
END
GO

-- Update existing DisplayOrder values if needed (set sequential order)
UPDATE [dbo].[GalleryImages]
SET [DisplayOrder] = ROW_NUMBER() OVER (PARTITION BY [GalleryId] ORDER BY [Id])
WHERE [DisplayOrder] = 0 OR [DisplayOrder] IS NULL;
GO

-- ============================================
-- Verify Changes
-- ============================================

PRINT '';
PRINT '========================================';
PRINT 'Galleries Table Structure:';
PRINT '========================================';
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
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
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'GalleryImages'
ORDER BY ORDINAL_POSITION;
GO

PRINT '';
PRINT '========================================';
PRINT 'Migration Completed Successfully!';
PRINT '========================================';
GO
