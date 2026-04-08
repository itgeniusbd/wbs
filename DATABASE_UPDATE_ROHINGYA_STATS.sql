-- =============================================
-- Add Rohingya Statistics Fields to SiteSettings
-- Migration: AddRohingyaStatisticsFields
-- Date: 2026-01-28
-- =============================================

USE [YourDatabaseName]
GO

PRINT 'Adding Rohingya Statistics Fields...'
GO

-- Add RohingyaCampsReached column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'RohingyaCampsReached')
BEGIN
    PRINT 'Adding RohingyaCampsReached column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [RohingyaCampsReached] INT NULL
    PRINT 'RohingyaCampsReached column added'
END
ELSE
BEGIN
    PRINT 'RohingyaCampsReached column already exists'
END
GO

-- Add RohingyaTotalBeneficiaries column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'RohingyaTotalBeneficiaries')
BEGIN
    PRINT 'Adding RohingyaTotalBeneficiaries column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [RohingyaTotalBeneficiaries] INT NULL
    PRINT 'RohingyaTotalBeneficiaries column added'
END
ELSE
BEGIN
    PRINT 'RohingyaTotalBeneficiaries column already exists'
END
GO

-- Add RohingyaActivePrograms column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'RohingyaActivePrograms')
BEGIN
    PRINT 'Adding RohingyaActivePrograms column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [RohingyaActivePrograms] INT NULL
    PRINT 'RohingyaActivePrograms column added'
END
ELSE
BEGIN
    PRINT 'RohingyaActivePrograms column already exists'
END
GO

-- Add RohingyaEventsConducted column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'RohingyaEventsConducted')
BEGIN
    PRINT 'Adding RohingyaEventsConducted column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [RohingyaEventsConducted] INT NULL
    PRINT 'RohingyaEventsConducted column added'
END
ELSE
BEGIN
    PRINT 'RohingyaEventsConducted column already exists'
END
GO

-- Insert migration history record
IF NOT EXISTS (SELECT * FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = '20260128000002_AddRohingyaStatisticsFields')
BEGIN
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260128000002_AddRohingyaStatisticsFields', '8.0.0')
    PRINT 'Migration history record added'
END
ELSE
BEGIN
    PRINT 'Migration already applied'
END
GO

PRINT 'Migration completed successfully!'
GO

-- Verify columns
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'SiteSettings'
    AND COLUMN_NAME LIKE 'Rohingya%'
ORDER BY COLUMN_NAME
GO

PRINT 'Done! ?'
GO
