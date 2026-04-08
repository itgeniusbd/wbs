-- ============================================
-- SMS Management System Tables Creation Script
-- Run this in SQL Server Management Studio
-- ============================================

USE [WBS_NGO]  -- Replace with your actual database name
GO

PRINT '=========================================='
PRINT 'Starting SMS System Tables Creation...'
PRINT '=========================================='

-- ============================================
-- Table 1: ContactGroups
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContactGroups]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ContactGroups](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [GroupName] [nvarchar](100) NOT NULL,
        [Description] [nvarchar](500) NULL,
        [IsActive] [bit] NOT NULL,
        [CreatedAt] [datetime2](7) NOT NULL,
     CONSTRAINT [PK_ContactGroups] PRIMARY KEY CLUSTERED ([Id] ASC)
    ) ON [PRIMARY]

    PRINT '? Table ContactGroups created successfully'
END
ELSE
BEGIN
    PRINT '? Table ContactGroups already exists'
END
GO

-- ============================================
-- Table 2: ContactListItems
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContactListItems]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ContactListItems](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        [PhoneNumber] [nvarchar](20) NOT NULL,
        [Email] [nvarchar](100) NULL,
        [Type] [nvarchar](50) NULL,
        [ContactGroupId] [int] NULL,
        [IsActive] [bit] NOT NULL,
        [CreatedAt] [datetime2](7) NOT NULL,
     CONSTRAINT [PK_ContactListItems] PRIMARY KEY CLUSTERED ([Id] ASC)
    ) ON [PRIMARY]

    PRINT '? Table ContactListItems created successfully'
END
ELSE
BEGIN
    PRINT '? Table ContactListItems already exists'
END
GO

-- ============================================
-- Table 3: SMSCampaigns
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSCampaigns]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[SMSCampaigns](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [CampaignName] [nvarchar](200) NOT NULL,
        [Message] [nvarchar](max) NOT NULL,
        [RecipientType] [nvarchar](50) NOT NULL,
        [ContactGroupId] [int] NULL,
        [TotalRecipients] [int] NOT NULL DEFAULT 0,
        [SuccessCount] [int] NOT NULL DEFAULT 0,
        [FailedCount] [int] NOT NULL DEFAULT 0,
        [Status] [nvarchar](50) NOT NULL DEFAULT 'Pending',
        [CreatedBy] [nvarchar](100) NULL,
        [CreatedAt] [datetime2](7) NOT NULL,
        [SentAt] [datetime2](7) NULL,
     CONSTRAINT [PK_SMSCampaigns] PRIMARY KEY CLUSTERED ([Id] ASC)
    ) ON [PRIMARY]

    PRINT '? Table SMSCampaigns created successfully'
END
ELSE
BEGIN
    PRINT '? Table SMSCampaigns already exists'
END
GO

-- ============================================
-- Table 4: SMSCampaignRecipients
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSCampaignRecipients]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[SMSCampaignRecipients](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [SMSCampaignId] [int] NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        [PhoneNumber] [nvarchar](20) NOT NULL,
        [Status] [nvarchar](50) NOT NULL DEFAULT 'Pending',
        [SentAt] [datetime2](7) NULL,
        [ErrorMessage] [nvarchar](500) NULL,
     CONSTRAINT [PK_SMSCampaignRecipients] PRIMARY KEY CLUSTERED ([Id] ASC)
    ) ON [PRIMARY]

    PRINT '? Table SMSCampaignRecipients created successfully'
END
ELSE
BEGIN
    PRINT '? Table SMSCampaignRecipients already exists'
END
GO

-- ============================================
-- Create Indexes
-- ============================================

-- ContactListItems indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ContactListItems_ContactGroupId' AND object_id = OBJECT_ID('ContactListItems'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ContactListItems_ContactGroupId] ON [dbo].[ContactListItems]
    (
        [ContactGroupId] ASC
    )
    PRINT '? Index IX_ContactListItems_ContactGroupId created'
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ContactListItems_PhoneNumber' AND object_id = OBJECT_ID('ContactListItems'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ContactListItems_PhoneNumber] ON [dbo].[ContactListItems]
    (
        [PhoneNumber] ASC
    )
    PRINT '? Index IX_ContactListItems_PhoneNumber created'
END
GO

-- SMSCampaigns indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SMSCampaigns_ContactGroupId' AND object_id = OBJECT_ID('SMSCampaigns'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_SMSCampaigns_ContactGroupId] ON [dbo].[SMSCampaigns]
    (
        [ContactGroupId] ASC
    )
    PRINT '? Index IX_SMSCampaigns_ContactGroupId created'
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SMSCampaigns_CreatedAt' AND object_id = OBJECT_ID('SMSCampaigns'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_SMSCampaigns_CreatedAt] ON [dbo].[SMSCampaigns]
    (
        [CreatedAt] DESC
    )
    PRINT '? Index IX_SMSCampaigns_CreatedAt created'
END
GO

-- SMSCampaignRecipients indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SMSCampaignRecipients_SMSCampaignId' AND object_id = OBJECT_ID('SMSCampaignRecipients'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_SMSCampaignRecipients_SMSCampaignId] ON [dbo].[SMSCampaignRecipients]
    (
        [SMSCampaignId] ASC
    )
    PRINT '? Index IX_SMSCampaignRecipients_SMSCampaignId created'
END
GO

-- ============================================
-- Create Foreign Keys
-- ============================================

-- ContactListItems -> ContactGroups
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ContactListItems_ContactGroups_ContactGroupId')
BEGIN
    ALTER TABLE [dbo].[ContactListItems] WITH CHECK ADD CONSTRAINT [FK_ContactListItems_ContactGroups_ContactGroupId] 
    FOREIGN KEY([ContactGroupId]) REFERENCES [dbo].[ContactGroups] ([Id])
    ON DELETE SET NULL
    
    ALTER TABLE [dbo].[ContactListItems] CHECK CONSTRAINT [FK_ContactListItems_ContactGroups_ContactGroupId]
    PRINT '? Foreign Key FK_ContactListItems_ContactGroups_ContactGroupId created'
END
GO

-- SMSCampaigns -> ContactGroups
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_SMSCampaigns_ContactGroups_ContactGroupId')
BEGIN
    ALTER TABLE [dbo].[SMSCampaigns] WITH CHECK ADD CONSTRAINT [FK_SMSCampaigns_ContactGroups_ContactGroupId] 
    FOREIGN KEY([ContactGroupId]) REFERENCES [dbo].[ContactGroups] ([Id])
    ON DELETE SET NULL
    
    ALTER TABLE [dbo].[SMSCampaigns] CHECK CONSTRAINT [FK_SMSCampaigns_ContactGroups_ContactGroupId]
    PRINT '? Foreign Key FK_SMSCampaigns_ContactGroups_ContactGroupId created'
END
GO

-- SMSCampaignRecipients -> SMSCampaigns
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_SMSCampaignRecipients_SMSCampaigns_SMSCampaignId')
BEGIN
    ALTER TABLE [dbo].[SMSCampaignRecipients] WITH CHECK ADD CONSTRAINT [FK_SMSCampaignRecipients_SMSCampaigns_SMSCampaignId] 
    FOREIGN KEY([SMSCampaignId]) REFERENCES [dbo].[SMSCampaigns] ([Id])
    ON DELETE CASCADE
    
    ALTER TABLE [dbo].[SMSCampaignRecipients] CHECK CONSTRAINT [FK_SMSCampaignRecipients_SMSCampaigns_SMSCampaignId]
    PRINT '? Foreign Key FK_SMSCampaignRecipients_SMSCampaigns_SMSCampaignId created'
END
GO

-- ============================================
-- Verify Tables
-- ============================================
PRINT ''
PRINT '=========================================='
PRINT 'Verification:'
PRINT '=========================================='

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContactGroups]') AND type in (N'U'))
    PRINT '? ContactGroups table exists'
ELSE
    PRINT '? ContactGroups table NOT found'

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContactListItems]') AND type in (N'U'))
    PRINT '? ContactListItems table exists'
ELSE
    PRINT '? ContactListItems table NOT found'

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSCampaigns]') AND type in (N'U'))
    PRINT '? SMSCampaigns table exists'
ELSE
    PRINT '? SMSCampaigns table NOT found'

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSCampaignRecipients]') AND type in (N'U'))
    PRINT '? SMSCampaignRecipients table exists'
ELSE
    PRINT '? SMSCampaignRecipients table NOT found'

PRINT ''
PRINT '=========================================='
PRINT 'SUCCESS: SMS System tables are ready!'
PRINT '=========================================='
PRINT ''
PRINT 'You can now:'
PRINT '1. Send SMS to Donors, Volunteers'
PRINT '2. Create Contact Groups'
PRINT '3. Manage Contact Lists'
PRINT '4. Track SMS Campaigns'
PRINT ''
PRINT 'Next steps:'
PRINT '- Update Admin Menu'
PRINT '- Create Views for SMS pages'
PRINT '=========================================='
GO

-- Show all SMS tables structure
SELECT 
    t.name AS TableName,
    c.name AS ColumnName,
    ty.name AS DataType,
    c.max_length AS MaxLength,
    c.is_nullable AS IsNullable
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
INNER JOIN sys.types ty ON c.user_type_id = ty.user_type_id
WHERE t.name IN ('ContactGroups', 'ContactListItems', 'SMSCampaigns', 'SMSCampaignRecipients')
ORDER BY t.name, c.column_id
GO
