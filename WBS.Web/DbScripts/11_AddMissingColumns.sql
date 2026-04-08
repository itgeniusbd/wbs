-- Add missing columns to DonationTypes, SDGs, and Sectors tables
USE WBS_NGO;
GO

PRINT '========================================';
PRINT 'Adding Missing Columns to Database';
PRINT '========================================';
PRINT ''

-- ========================================
-- DonationTypes table
-- ========================================
PRINT 'Processing DonationTypes table...'
PRINT ''

-- Add DescriptionBn column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DonationTypes' AND COLUMN_NAME = 'DescriptionBn')
BEGIN
    ALTER TABLE DonationTypes ADD DescriptionBn NVARCHAR(500) NULL;
    PRINT '? DescriptionBn column added to DonationTypes';
END
ELSE
    PRINT '? DescriptionBn column already exists in DonationTypes';

-- Add Icon column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DonationTypes' AND COLUMN_NAME = 'Icon')
BEGIN
    ALTER TABLE DonationTypes ADD Icon NVARCHAR(200) NULL;
    PRINT '? Icon column added to DonationTypes';
END
ELSE
    PRINT '? Icon column already exists in DonationTypes';

-- Add Image column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DonationTypes' AND COLUMN_NAME = 'Image')
BEGIN
    ALTER TABLE DonationTypes ADD [Image] NVARCHAR(500) NULL;
    PRINT '? Image column added to DonationTypes';
END
ELSE
    PRINT '? Image column already exists in DonationTypes';

-- ========================================
-- SDGs table
-- ========================================
PRINT ''
PRINT 'Processing SDGs table...'
PRINT ''

-- Add Description column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SDGs' AND COLUMN_NAME = 'Description')
BEGIN
    ALTER TABLE SDGs ADD [Description] NVARCHAR(MAX) NULL;
    PRINT '? Description column added to SDGs';
END
ELSE
    PRINT '? Description column already exists in SDGs';

-- Add DescriptionBn column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SDGs' AND COLUMN_NAME = 'DescriptionBn')
BEGIN
    ALTER TABLE SDGs ADD DescriptionBn NVARCHAR(MAX) NULL;
    PRINT '? DescriptionBn column added to SDGs';
END
ELSE
    PRINT '? DescriptionBn column already exists in SDGs';

-- Add Icon column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SDGs' AND COLUMN_NAME = 'Icon')
BEGIN
    ALTER TABLE SDGs ADD Icon NVARCHAR(200) NULL;
    PRINT '? Icon column added to SDGs';
END
ELSE
    PRINT '? Icon column already exists in SDGs';

-- ========================================
-- Sectors table
-- ========================================
PRINT ''
PRINT 'Processing Sectors table...'
PRINT ''

-- Add Description column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sectors' AND COLUMN_NAME = 'Description')
BEGIN
    ALTER TABLE Sectors ADD [Description] NVARCHAR(MAX) NULL;
    PRINT '? Description column added to Sectors';
END
ELSE
    PRINT '? Description column already exists in Sectors';

-- Add DescriptionBn column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sectors' AND COLUMN_NAME = 'DescriptionBn')
BEGIN
    ALTER TABLE Sectors ADD DescriptionBn NVARCHAR(MAX) NULL;
    PRINT '? DescriptionBn column added to Sectors';
END
ELSE
    PRINT '? DescriptionBn column already exists in Sectors';

-- Add Icon column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sectors' AND COLUMN_NAME = 'Icon')
BEGIN
    ALTER TABLE Sectors ADD Icon NVARCHAR(200) NULL;
    PRINT '? Icon column added to Sectors';
END
ELSE
    PRINT '? Icon column already exists in Sectors';

-- Add Image column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sectors' AND COLUMN_NAME = 'Image')
BEGIN
    ALTER TABLE Sectors ADD [Image] NVARCHAR(500) NULL;
    PRINT '? Image column added to Sectors';
END
ELSE
    PRINT '? Image column already exists in Sectors';

-- Add SDGId column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sectors' AND COLUMN_NAME = 'SDGId')
BEGIN
    ALTER TABLE Sectors ADD SDGId INT NULL;
    PRINT '? SDGId column added to Sectors';
    
    -- Add foreign key constraint
    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Sectors_SDGs_SDGId')
    BEGIN
        ALTER TABLE Sectors ADD CONSTRAINT FK_Sectors_SDGs_SDGId 
            FOREIGN KEY (SDGId) REFERENCES SDGs(Id);
        PRINT '? Foreign key constraint added';
    END
END
ELSE
    PRINT '? SDGId column already exists in Sectors';

-- Add CreatedAt column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sectors' AND COLUMN_NAME = 'CreatedAt')
BEGIN
    ALTER TABLE Sectors ADD CreatedAt DATETIME2(7) NOT NULL DEFAULT GETUTCDATE();
    PRINT '? CreatedAt column added to Sectors';
END
ELSE
    PRINT '? CreatedAt column already exists in Sectors';

PRINT ''
PRINT '========================================';
PRINT '? All tables updated successfully!';
PRINT '========================================';
PRINT ''

-- ========================================
-- Verification
-- ========================================
PRINT 'Verifying DonationTypes columns:'
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'DonationTypes'
    AND COLUMN_NAME IN ('DescriptionBn', 'Icon', 'Image')
ORDER BY COLUMN_NAME;

PRINT ''
PRINT 'Verifying SDGs columns:'
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'SDGs'
    AND COLUMN_NAME IN ('Description', 'DescriptionBn', 'Icon')
ORDER BY COLUMN_NAME;

PRINT ''
PRINT 'Verifying Sectors columns:'
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Sectors'
    AND COLUMN_NAME IN ('Description', 'DescriptionBn', 'Icon', 'Image', 'SDGId', 'CreatedAt')
ORDER BY COLUMN_NAME;
GO
