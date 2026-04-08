-- Add missing columns to AspNetUsers table
USE WBS_NGO;
GO

PRINT 'Adding missing columns to AspNetUsers table...'
PRINT ''

-- Add FirstName column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'FirstName')
BEGIN
    ALTER TABLE AspNetUsers ADD FirstName NVARCHAR(100) NOT NULL DEFAULT '';
    PRINT '? FirstName column added';
END
ELSE
    PRINT '? FirstName column already exists';

-- Add LastName column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'LastName')
BEGIN
    ALTER TABLE AspNetUsers ADD LastName NVARCHAR(100) NOT NULL DEFAULT '';
    PRINT '? LastName column added';
END
ELSE
    PRINT '? LastName column already exists';

-- Add FirstNameBn column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'FirstNameBn')
BEGIN
    ALTER TABLE AspNetUsers ADD FirstNameBn NVARCHAR(100) NULL;
    PRINT '? FirstNameBn column added';
END
ELSE
    PRINT '? FirstNameBn column already exists';

-- Add LastNameBn column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'LastNameBn')
BEGIN
    ALTER TABLE AspNetUsers ADD LastNameBn NVARCHAR(100) NULL;
    PRINT '? LastNameBn column added';
END
ELSE
    PRINT '? LastNameBn column already exists';

-- Add ProfilePicture column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'ProfilePicture')
BEGIN
    ALTER TABLE AspNetUsers ADD ProfilePicture NVARCHAR(500) NULL;
    PRINT '? ProfilePicture column added';
END
ELSE
    PRINT '? ProfilePicture column already exists';

-- Add IsActive column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'IsActive')
BEGIN
    ALTER TABLE AspNetUsers ADD IsActive BIT NOT NULL DEFAULT 1;
    PRINT '? IsActive column added';
END
ELSE
    PRINT '? IsActive column already exists';

-- Add CreatedAt column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'CreatedAt')
BEGIN
    ALTER TABLE AspNetUsers ADD CreatedAt DATETIME2(7) NOT NULL DEFAULT GETUTCDATE();
    PRINT '? CreatedAt column added';
END
ELSE
    PRINT '? CreatedAt column already exists';

-- Add LastLoginAt column if not exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'LastLoginAt')
BEGIN
    ALTER TABLE AspNetUsers ADD LastLoginAt DATETIME2(7) NULL;
    PRINT '? LastLoginAt column added';
END
ELSE
    PRINT '? LastLoginAt column already exists';

PRINT ''
PRINT '? AspNetUsers table updated successfully!'
PRINT ''

-- Verify the columns
PRINT 'Verifying AspNetUsers columns:'
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'AspNetUsers'
    AND COLUMN_NAME IN (
        'FirstName', 'LastName', 'FirstNameBn', 'LastNameBn',
        'ProfilePicture', 'IsActive', 'CreatedAt', 'LastLoginAt'
    )
ORDER BY COLUMN_NAME;
GO
