-- Fix Events table structure by removing old EventDate column if exists
-- and ensuring StartDate exists

USE [WBS_NGO]
GO

PRINT '=== Starting Events Table Structure Fix ==='
GO

-- Check current structure
PRINT 'Current table structure:'
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Events'
ORDER BY ORDINAL_POSITION;
GO

-- Check if old EventDate column exists and remove it
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
           WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'EventDate')
BEGIN
    PRINT 'Found EventDate column - migrating data...'
    
    -- First, ensure StartDate column exists
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                   WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'StartDate')
    BEGIN
        ALTER TABLE Events ADD StartDate DATETIME2 NOT NULL DEFAULT GETUTCDATE();
        PRINT 'StartDate column created';
    END
    
    -- Copy data from EventDate to StartDate if StartDate has default values
    UPDATE Events 
    SET StartDate = EventDate 
    WHERE EventDate IS NOT NULL 
      AND (StartDate = '0001-01-01' OR StartDate = CAST('1900-01-01' AS DATETIME2));
    PRINT 'Data copied from EventDate to StartDate';
    
    -- Now remove the EventDate column
    ALTER TABLE Events DROP COLUMN EventDate;
    PRINT '✓ EventDate column removed successfully';
END
ELSE
BEGIN
    PRINT '✓ EventDate column does not exist - table structure is correct';
END
GO

-- Ensure all required columns exist
PRINT 'Checking for missing columns...'

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'StartDate')
BEGIN
    ALTER TABLE Events ADD StartDate DATETIME2 NOT NULL DEFAULT GETUTCDATE();
    PRINT '✓ StartDate column added';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'EndDate')
BEGIN
    ALTER TABLE Events ADD EndDate DATETIME2 NULL;
    PRINT '✓ EndDate column added';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'TicketPrice')
BEGIN
    ALTER TABLE Events ADD TicketPrice DECIMAL(18,2) NULL;
    PRINT '✓ TicketPrice column added';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'TotalCapacity')
BEGIN
    ALTER TABLE Events ADD TotalCapacity INT NULL;
    PRINT '✓ TotalCapacity column added';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'RegistrationDeadline')
BEGIN
    ALTER TABLE Events ADD RegistrationDeadline DATETIME2 NULL;
    PRINT '✓ RegistrationDeadline column added';
END
GO

-- Verify the final structure
PRINT ''
PRINT '=== Final Table Structure ==='
SELECT 
    COLUMN_NAME as 'Column Name', 
    DATA_TYPE as 'Data Type', 
    IS_NULLABLE as 'Nullable',
    COLUMN_DEFAULT as 'Default Value'
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Events'
ORDER BY ORDINAL_POSITION;
GO

PRINT ''
PRINT '=== Events table structure fixed successfully! ==='
PRINT 'You can now create events through the Admin Panel.'
GO
