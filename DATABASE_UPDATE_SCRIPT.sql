-- =============================================
-- Database Update Script
-- Migration: AddOrganizationAndPolicyFields
-- Date: 2026-01-28
-- =============================================

USE [YourDatabaseName]
GO

-- Check if columns already exist before adding them
-- This script is safe to run multiple times

PRINT 'Starting Migration: AddOrganizationAndPolicyFields'
GO

-- Add OrganizationFullName column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'OrganizationFullName')
BEGIN
    PRINT 'Adding OrganizationFullName column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [OrganizationFullName] NVARCHAR(200) NULL
    PRINT 'OrganizationFullName column added successfully'
END
ELSE
BEGIN
    PRINT 'OrganizationFullName column already exists'
END
GO

-- Add OrganizationFullNameBn column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'OrganizationFullNameBn')
BEGIN
    PRINT 'Adding OrganizationFullNameBn column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [OrganizationFullNameBn] NVARCHAR(200) NULL
    PRINT 'OrganizationFullNameBn column added successfully'
END
ELSE
BEGIN
    PRINT 'OrganizationFullNameBn column already exists'
END
GO

-- Add RegistrationNumber column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'RegistrationNumber')
BEGIN
    PRINT 'Adding RegistrationNumber column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [RegistrationNumber] NVARCHAR(100) NULL
    PRINT 'RegistrationNumber column added successfully'
END
ELSE
BEGIN
    PRINT 'RegistrationNumber column already exists'
END
GO

-- Add RegistrationType column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'RegistrationType')
BEGIN
    PRINT 'Adding RegistrationType column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [RegistrationType] NVARCHAR(100) NULL
    PRINT 'RegistrationType column added successfully'
END
ELSE
BEGIN
    PRINT 'RegistrationType column already exists'
END
GO

-- Add EstablishedYear column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'EstablishedYear')
BEGIN
    PRINT 'Adding EstablishedYear column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [EstablishedYear] INT NULL
    PRINT 'EstablishedYear column added successfully'
END
ELSE
BEGIN
    PRINT 'EstablishedYear column already exists'
END
GO

-- Add OrganizationType column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'OrganizationType')
BEGIN
    PRINT 'Adding OrganizationType column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [OrganizationType] NVARCHAR(MAX) NULL
    PRINT 'OrganizationType column added successfully'
END
ELSE
BEGIN
    PRINT 'OrganizationType column already exists'
END
GO

-- Add OrganizationTypeBn column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'OrganizationTypeBn')
BEGIN
    PRINT 'Adding OrganizationTypeBn column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [OrganizationTypeBn] NVARCHAR(MAX) NULL
    PRINT 'OrganizationTypeBn column added successfully'
END
ELSE
BEGIN
    PRINT 'OrganizationTypeBn column already exists'
END
GO

-- Add ManagementInfo column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'ManagementInfo')
BEGIN
    PRINT 'Adding ManagementInfo column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [ManagementInfo] NVARCHAR(MAX) NULL
    PRINT 'ManagementInfo column added successfully'
END
ELSE
BEGIN
    PRINT 'ManagementInfo column already exists'
END
GO

-- Add ManagementInfoBn column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'ManagementInfoBn')
BEGIN
    PRINT 'Adding ManagementInfoBn column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [ManagementInfoBn] NVARCHAR(MAX) NULL
    PRINT 'ManagementInfoBn column added successfully'
END
ELSE
BEGIN
    PRINT 'ManagementInfoBn column already exists'
END
GO

-- Add RefundPolicyTimeframe column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'RefundPolicyTimeframe')
BEGIN
    PRINT 'Adding RefundPolicyTimeframe column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [RefundPolicyTimeframe] NVARCHAR(500) NULL
    PRINT 'RefundPolicyTimeframe column added successfully'
END
ELSE
BEGIN
    PRINT 'RefundPolicyTimeframe column already exists'
END
GO

-- Add RefundPolicyTimeframeBn column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'RefundPolicyTimeframeBn')
BEGIN
    PRINT 'Adding RefundPolicyTimeframeBn column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [RefundPolicyTimeframeBn] NVARCHAR(500) NULL
    PRINT 'RefundPolicyTimeframeBn column added successfully'
END
ELSE
BEGIN
    PRINT 'RefundPolicyTimeframeBn column already exists'
END
GO

-- Add PaymentGatewayBanner column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND name = 'PaymentGatewayBanner')
BEGIN
    PRINT 'Adding PaymentGatewayBanner column...'
    ALTER TABLE [dbo].[SiteSettings]
    ADD [PaymentGatewayBanner] NVARCHAR(MAX) NULL
    PRINT 'PaymentGatewayBanner column added successfully'
END
ELSE
BEGIN
    PRINT 'PaymentGatewayBanner column already exists'
END
GO

-- Insert migration history record
IF NOT EXISTS (SELECT * FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = '20260128000001_AddOrganizationAndPolicyFields')
BEGIN
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260128000001_AddOrganizationAndPolicyFields', '8.0.0')
    PRINT 'Migration history record added'
END
ELSE
BEGIN
    PRINT 'Migration history record already exists'
END
GO

PRINT 'Migration completed successfully!'
GO

-- Verify the changes
PRINT 'Verifying columns...'
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'SiteSettings'
    AND COLUMN_NAME IN (
        'OrganizationFullName',
        'OrganizationFullNameBn',
        'RegistrationNumber',
        'RegistrationType',
        'EstablishedYear',
        'OrganizationType',
        'OrganizationTypeBn',
        'ManagementInfo',
        'ManagementInfoBn',
        'RefundPolicyTimeframe',
        'RefundPolicyTimeframeBn',
        'PaymentGatewayBanner'
    )
ORDER BY COLUMN_NAME
GO

PRINT 'All done! ?'
GO
