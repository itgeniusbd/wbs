-- =============================================
-- Quick Update: Set Rohingya Camps Reached to 5
-- This script will immediately update the camps count
-- =============================================

USE [WBS_NGO]  -- Change to your database name
GO

PRINT 'Step 1: Removing old incorrect columns...'
GO

-- Drop old column with Cyrillic character (if exists)
IF EXISTS (SELECT * FROM sys.columns 
           WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') 
           AND name LIKE 'RohingyaEvents%')
BEGIN
    -- Find and drop the incorrect column
    DECLARE @sql NVARCHAR(MAX)
    SELECT @sql = 'ALTER TABLE [dbo].[SiteSettings] DROP COLUMN [' + COLUMN_NAME + ']'
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'SiteSettings' 
    AND COLUMN_NAME LIKE 'RohingyaEvents%'
    AND COLUMN_NAME != 'RohingyaEventsConducted'
    
    IF @sql IS NOT NULL
    BEGIN
        EXEC sp_executesql @sql
        PRINT 'Old incorrect column dropped'
    END
END
GO

PRINT 'Step 2: Adding correct columns...'
GO

-- Add RohingyaCampsReached (if not exists)
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') 
               AND name = 'RohingyaCampsReached')
BEGIN
    ALTER TABLE [dbo].[SiteSettings] ADD [RohingyaCampsReached] INT NULL
    PRINT 'RohingyaCampsReached column added'
END
GO

-- Add RohingyaTotalBeneficiaries (if not exists)
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') 
               AND name = 'RohingyaTotalBeneficiaries')
BEGIN
    ALTER TABLE [dbo].[SiteSettings] ADD [RohingyaTotalBeneficiaries] INT NULL
    PRINT 'RohingyaTotalBeneficiaries column added'
END
GO

-- Add RohingyaActivePrograms (if not exists)
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') 
               AND name = 'RohingyaActivePrograms')
BEGIN
    ALTER TABLE [dbo].[SiteSettings] ADD [RohingyaActivePrograms] INT NULL
    PRINT 'RohingyaActivePrograms column added'
END
GO

-- Add RohingyaEventsConducted (if not exists)
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') 
               AND name = 'RohingyaEventsConducted')
BEGIN
    ALTER TABLE [dbo].[SiteSettings] ADD [RohingyaEventsConducted] INT NULL
    PRINT 'RohingyaEventsConducted column added'
END
GO

PRINT 'Step 3: Updating Rohingya Statistics...'
GO

-- Update the camps reached value
UPDATE [dbo].[SiteSettings]
SET [RohingyaCampsReached] = 5,
    [RohingyaTotalBeneficiaries] = 2250,
    [RohingyaActivePrograms] = 7,
    [RohingyaEventsConducted] = 3,
    [UpdatedAt] = GETDATE()
WHERE Id = (SELECT TOP 1 Id FROM [dbo].[SiteSettings])
GO

PRINT 'Step 4: Verifying the update...'
GO

-- Verify the update
SELECT 
    Id,
    RohingyaCampsReached AS 'Camps Reached',
    RohingyaTotalBeneficiaries AS 'Total Beneficiaries',
    RohingyaActivePrograms AS 'Active Programs',
    RohingyaEventsConducted AS 'Events Conducted',
    UpdatedAt AS 'Last Updated'
FROM [dbo].[SiteSettings]
GO

PRINT 'All Rohingya statistics updated successfully! ?'
GO
