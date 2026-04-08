-- Add missing columns to SiteSettings table
USE WBS_NGO;
GO

PRINT '========================================';
PRINT 'Adding Missing Columns to SiteSettings';
PRINT '========================================';
PRINT ''

-- AboutUs columns
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'AboutUs')
BEGIN
    ALTER TABLE SiteSettings ADD AboutUs NVARCHAR(MAX) NULL;
    PRINT '? AboutUs column added';
END
ELSE PRINT '? AboutUs already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'AboutUsBn')
BEGIN
    ALTER TABLE SiteSettings ADD AboutUsBn NVARCHAR(MAX) NULL;
    PRINT '? AboutUsBn column added';
END
ELSE PRINT '? AboutUsBn already exists';

-- Bank Details
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'BankAccountName')
BEGIN
    ALTER TABLE SiteSettings ADD BankAccountName NVARCHAR(200) NULL;
    PRINT '? BankAccountName column added';
END
ELSE PRINT '? BankAccountName already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'BankAccountNumber')
BEGIN
    ALTER TABLE SiteSettings ADD BankAccountNumber NVARCHAR(100) NULL;
    PRINT '? BankAccountNumber column added';
END
ELSE PRINT '? BankAccountNumber already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'BankBranchName')
BEGIN
    ALTER TABLE SiteSettings ADD BankBranchName NVARCHAR(200) NULL;
    PRINT '? BankBranchName column added';
END
ELSE PRINT '? BankBranchName already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'BankName')
BEGIN
    ALTER TABLE SiteSettings ADD BankName NVARCHAR(200) NULL;
    PRINT '? BankName column added';
END
ELSE PRINT '? BankName already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'BankRoutingNumber')
BEGIN
    ALTER TABLE SiteSettings ADD BankRoutingNumber NVARCHAR(100) NULL;
    PRINT '? BankRoutingNumber column added';
END
ELSE PRINT '? BankRoutingNumber already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'BankSwiftCode')
BEGIN
    ALTER TABLE SiteSettings ADD BankSwiftCode NVARCHAR(100) NULL;
    PRINT '? BankSwiftCode column added';
END
ELSE PRINT '? BankSwiftCode already exists';

-- Mobile Banking
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'BkashNumber')
BEGIN
    ALTER TABLE SiteSettings ADD BkashNumber NVARCHAR(50) NULL;
    PRINT '? BkashNumber column added';
END
ELSE PRINT '? BkashNumber already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'NagadNumber')
BEGIN
    ALTER TABLE SiteSettings ADD NagadNumber NVARCHAR(50) NULL;
    PRINT '? NagadNumber column added';
END
ELSE PRINT '? NagadNumber already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'RocketNumber')
BEGIN
    ALTER TABLE SiteSettings ADD RocketNumber NVARCHAR(50) NULL;
    PRINT '? RocketNumber column added';
END
ELSE PRINT '? RocketNumber already exists';

-- Other columns
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'CopyrightText')
BEGIN
    ALTER TABLE SiteSettings ADD CopyrightText NVARCHAR(500) NULL;
    PRINT '? CopyrightText column added';
END
ELSE PRINT '? CopyrightText already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'Favicon')
BEGIN
    ALTER TABLE SiteSettings ADD Favicon NVARCHAR(500) NULL;
    PRINT '? Favicon column added';
END
ELSE PRINT '? Favicon already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'GoogleMapEmbed')
BEGIN
    ALTER TABLE SiteSettings ADD GoogleMapEmbed NVARCHAR(MAX) NULL;
    PRINT '? GoogleMapEmbed column added';
END
ELSE PRINT '? GoogleMapEmbed already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'Logo')
BEGIN
    ALTER TABLE SiteSettings ADD Logo NVARCHAR(500) NULL;
    PRINT '? Logo column added';
END
ELSE PRINT '? Logo already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'LogoWhite')
BEGIN
    ALTER TABLE SiteSettings ADD LogoWhite NVARCHAR(500) NULL;
    PRINT '? LogoWhite column added';
END
ELSE PRINT '? LogoWhite already exists';

-- Meta columns
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'MetaDescription')
BEGIN
    ALTER TABLE SiteSettings ADD MetaDescription NVARCHAR(500) NULL;
    PRINT '? MetaDescription column added';
END
ELSE PRINT '? MetaDescription already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'MetaKeywords')
BEGIN
    ALTER TABLE SiteSettings ADD MetaKeywords NVARCHAR(500) NULL;
    PRINT '? MetaKeywords column added';
END
ELSE PRINT '? MetaKeywords already exists';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'MetaTitle')
BEGIN
    ALTER TABLE SiteSettings ADD MetaTitle NVARCHAR(200) NULL;
    PRINT '? MetaTitle column added';
END
ELSE PRINT '? MetaTitle already exists';

-- UpdatedAt
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SiteSettings' AND COLUMN_NAME = 'UpdatedAt')
BEGIN
    ALTER TABLE SiteSettings ADD UpdatedAt DATETIME2(7) NOT NULL DEFAULT GETUTCDATE();
    PRINT '? UpdatedAt column added';
END
ELSE PRINT '? UpdatedAt already exists';

PRINT ''
PRINT '========================================';
PRINT '? SiteSettings table updated!';
PRINT '========================================';
PRINT ''

-- Verification
PRINT 'Verifying all columns:'
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'SiteSettings'
ORDER BY COLUMN_NAME;
GO
