-- Add missing columns to Sliders table
USE WBS_NGO;
GO

PRINT '========================================';
PRINT 'Adding Missing Columns to Sliders Table';
PRINT '========================================';
PRINT ''

-- Add ButtonUrl column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sliders' AND COLUMN_NAME = 'ButtonUrl')
BEGIN
    ALTER TABLE Sliders ADD ButtonUrl NVARCHAR(500) NULL;
    PRINT '? ButtonUrl column added to Sliders';
END
ELSE
    PRINT '? ButtonUrl column already exists in Sliders';

-- Add SecondButtonText column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sliders' AND COLUMN_NAME = 'SecondButtonText')
BEGIN
    ALTER TABLE Sliders ADD SecondButtonText NVARCHAR(200) NULL;
    PRINT '? SecondButtonText column added to Sliders';
END
ELSE
    PRINT '? SecondButtonText column already exists in Sliders';

-- Add SecondButtonUrl column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sliders' AND COLUMN_NAME = 'SecondButtonUrl')
BEGIN
    ALTER TABLE Sliders ADD SecondButtonUrl NVARCHAR(500) NULL;
    PRINT '? SecondButtonUrl column added to Sliders';
END
ELSE
    PRINT '? SecondButtonUrl column already exists in Sliders';

-- Add CreatedAt column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sliders' AND COLUMN_NAME = 'CreatedAt')
BEGIN
    ALTER TABLE Sliders ADD CreatedAt DATETIME2(7) NOT NULL DEFAULT GETUTCDATE();
    PRINT '? CreatedAt column added to Sliders';
END
ELSE
    PRINT '? CreatedAt column already exists in Sliders';

PRINT ''
PRINT '========================================';
PRINT '? Sliders table updated successfully!';
PRINT '========================================';
PRINT ''

-- Verification
PRINT 'Verifying Sliders columns:'
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Sliders'
    AND COLUMN_NAME IN ('ButtonUrl', 'SecondButtonText', 'SecondButtonUrl', 'CreatedAt')
ORDER BY COLUMN_NAME;
GO
