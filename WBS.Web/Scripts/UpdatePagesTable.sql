-- Add missing columns to Pages table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'BannerImage')
BEGIN
    ALTER TABLE [dbo].[Pages] ADD [BannerImage] nvarchar(max) NULL;
    PRINT 'Added BannerImage column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'CreatedBy')
BEGIN
    ALTER TABLE [dbo].[Pages] ADD [CreatedBy] nvarchar(max) NULL;
    PRINT 'Added CreatedBy column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'FeaturedImage')
BEGIN
    ALTER TABLE [dbo].[Pages] ADD [FeaturedImage] nvarchar(max) NULL;
    PRINT 'Added FeaturedImage column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'MetaKeywords')
BEGIN
    ALTER TABLE [dbo].[Pages] ADD [MetaKeywords] nvarchar(max) NULL;
    PRINT 'Added MetaKeywords column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'ShowInFooter')
BEGIN
    ALTER TABLE [dbo].[Pages] ADD [ShowInFooter] bit NOT NULL DEFAULT 0;
    PRINT 'Added ShowInFooter column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND name = 'UpdatedBy')
BEGIN
    ALTER TABLE [dbo].[Pages] ADD [UpdatedBy] nvarchar(max) NULL;
    PRINT 'Added UpdatedBy column';
END

PRINT 'Pages table updated successfully!';
