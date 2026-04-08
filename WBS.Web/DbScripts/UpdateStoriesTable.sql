-- Add missing columns to Stories table
-- Run this script if you get errors about missing columns

USE [WBS_NGO]
GO

-- Check if columns exist before adding them
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Stories]') AND name = 'BeneficiaryName')
BEGIN
    ALTER TABLE [dbo].[Stories]
    ADD [BeneficiaryName] NVARCHAR(200) NULL
    PRINT 'Added BeneficiaryName column'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Stories]') AND name = 'BeneficiaryNameBn')
BEGIN
    ALTER TABLE [dbo].[Stories]
    ADD [BeneficiaryNameBn] NVARCHAR(200) NULL
    PRINT 'Added BeneficiaryNameBn column'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Stories]') AND name = 'Summary')
BEGIN
    ALTER TABLE [dbo].[Stories]
    ADD [Summary] NVARCHAR(MAX) NULL
    PRINT 'Added Summary column'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Stories]') AND name = 'SummaryBn')
BEGIN
    ALTER TABLE [dbo].[Stories]
    ADD [SummaryBn] NVARCHAR(MAX) NULL
    PRINT 'Added SummaryBn column'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Stories]') AND name = 'Location')
BEGIN
    ALTER TABLE [dbo].[Stories]
    ADD [Location] NVARCHAR(200) NULL
    PRINT 'Added Location column'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Stories]') AND name = 'LocationBn')
BEGIN
    ALTER TABLE [dbo].[Stories]
    ADD [LocationBn] NVARCHAR(200) NULL
    PRINT 'Added LocationBn column'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Stories]') AND name = 'VideoUrl')
BEGIN
    ALTER TABLE [dbo].[Stories]
    ADD [VideoUrl] NVARCHAR(500) NULL
    PRINT 'Added VideoUrl column'
END
GO

PRINT 'Stories table update completed successfully!'
GO
