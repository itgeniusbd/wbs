-- =============================================
-- Script: Add AboutUsImage to SiteSettings Table
-- Description: Adds AboutUsImage column to store the About Us section image URL
-- Date: 2024
-- =============================================

USE [WBS_DB]; -- Replace with your database name
GO

-- Check if column exists before adding
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') 
    AND name = 'AboutUsImage'
)
BEGIN
    -- Add AboutUsImage column
    ALTER TABLE [dbo].[SiteSettings]
    ADD [AboutUsImage] NVARCHAR(500) NULL;
    
    PRINT 'AboutUsImage column added to SiteSettings table successfully.';
    
    -- Optional: Set a default Cloudinary image URL
    UPDATE [dbo].[SiteSettings]
    SET [AboutUsImage] = 'https://res.cloudinary.com/dybngfiu0/image/upload/v1/wbs/about-us.jpg'
    WHERE [AboutUsImage] IS NULL;
    
    PRINT 'Default AboutUsImage URL set.';
END
ELSE
BEGIN
    PRINT 'AboutUsImage column already exists in SiteSettings table.';
END
GO

-- Verify the change
SELECT 
    Id,
    SiteName,
    AboutUsImage,
    UpdatedAt
FROM [dbo].[SiteSettings];
GO

PRINT 'Migration completed successfully!';
GO
