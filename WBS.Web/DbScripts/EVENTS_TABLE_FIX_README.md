# Events Table Structure Fix - Step by Step Guide

## Problem
The database has an old `EventDate` column that is causing errors. The application expects `StartDate` and `EndDate` columns instead.

Error: `Cannot insert the value NULL into column 'EventDate', table 'WBS_NGO.dbo.Events', column does not allow nulls`

## Solution Options

### Option 1: Quick Fix - Run SQL Script in SSMS (RECOMMENDED)

1. **Open SQL Server Management Studio (SSMS)**
   - Connect to server: `DESKTOP-3UN61QI`
   - Database: `WBS_NGO`

2. **Open and Run this script:**
   File location: `WBS.Web\DbScripts\FixEventsTableStructure.sql`
   
   Or copy-paste this:

```sql
-- Fix Events table structure
USE [WBS_NGO]
GO

-- Check current structure
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Events'
ORDER BY ORDINAL_POSITION;
GO

-- If EventDate exists, migrate data and remove it
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
           WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'EventDate')
BEGIN
    PRINT 'Found EventDate column - migrating data...';
    
    -- Migrate data from EventDate to StartDate
    UPDATE Events 
    SET StartDate = EventDate 
    WHERE EventDate IS NOT NULL;
    
    -- Remove the old column
    ALTER TABLE Events DROP COLUMN EventDate;
    PRINT 'EventDate column removed successfully';
END
ELSE
BEGIN
    PRINT 'EventDate column does not exist - structure is correct';
END
GO

-- Verify all required columns exist
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'TicketPrice')
BEGIN
    ALTER TABLE Events ADD TicketPrice DECIMAL(18,2) NULL;
    PRINT 'TicketPrice column added';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'TotalCapacity')
BEGIN
    ALTER TABLE Events ADD TotalCapacity INT NULL;
    PRINT 'TotalCapacity column added';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'RegistrationDeadline')
BEGIN
    ALTER TABLE Events ADD RegistrationDeadline DATETIME2 NULL;
    PRINT 'RegistrationDeadline column added';
END
GO

-- Show final structure
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Events'
ORDER BY ORDINAL_POSITION;
GO

PRINT 'Events table structure fixed successfully!';
```

3. **Execute the script** (Press F5 or click Execute)

4. **Restart your application**

### Option 2: Drop and Recreate Table (USE WITH CAUTION - DELETES DATA)

?? **WARNING: This will delete all existing events!**

```sql
USE [WBS_NGO]
GO

-- Backup existing data (optional)
SELECT * INTO Events_Backup FROM Events;
GO

-- Drop existing tables
DROP TABLE IF EXISTS EventRegistrations;
DROP TABLE IF EXISTS Events;
GO

-- Create fresh Events table
CREATE TABLE [dbo].[Events] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(300) NOT NULL,
    [TitleBn] NVARCHAR(300) NULL,
    [Slug] NVARCHAR(300) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [DescriptionBn] NVARCHAR(MAX) NULL,
    [FeaturedImage] NVARCHAR(500) NULL,
    [Location] NVARCHAR(200) NULL,
    [LocationBn] NVARCHAR(200) NULL,
    [StartDate] DATETIME2 NOT NULL,
    [EndDate] DATETIME2 NULL,
    [TicketPrice] DECIMAL(18,2) NULL,
    [TotalCapacity] INT NULL,
    [RegistrationDeadline] DATETIME2 NULL,
    [RegistrationUrl] NVARCHAR(500) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [IsFeatured] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Create EventRegistrations table
CREATE TABLE [dbo].[EventRegistrations] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [EventId] INT NOT NULL,
    [FullName] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [Phone] NVARCHAR(20) NOT NULL,
    [Address] NVARCHAR(200) NULL,
    [Organization] NVARCHAR(100) NULL,
    [AmountPaid] DECIMAL(18,2) NOT NULL DEFAULT 0,
    [PaymentMethod] NVARCHAR(50) NOT NULL,
    [TransactionId] NVARCHAR(100) NULL,
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    [Notes] NVARCHAR(MAX) NULL,
    [RegisteredAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ConfirmedAt] DATETIME2 NULL,
    CONSTRAINT [FK_EventRegistrations_Events] FOREIGN KEY ([EventId]) REFERENCES [Events]([Id]) ON DELETE CASCADE
);
GO

PRINT 'Events table recreated successfully!';
```

## Verify the Fix

After running the script, verify the structure:

```sql
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Events'
ORDER BY ORDINAL_POSITION;
```

Expected columns:
- Id
- Title (NVARCHAR, NOT NULL)
- TitleBn (NVARCHAR, NULL)
- Slug (NVARCHAR, NOT NULL)
- Description (NVARCHAR(MAX), NULL)
- DescriptionBn (NVARCHAR(MAX), NULL)
- FeaturedImage (NVARCHAR, NULL)
- Location (NVARCHAR, NULL)
- LocationBn (NVARCHAR, NULL)
- **StartDate** (DATETIME2, NOT NULL) ?
- **EndDate** (DATETIME2, NULL) ?
- **TicketPrice** (DECIMAL, NULL) ?
- **TotalCapacity** (INT, NULL) ?
- **RegistrationDeadline** (DATETIME2, NULL) ?
- RegistrationUrl (NVARCHAR, NULL)
- IsActive (BIT, NOT NULL)
- IsFeatured (BIT, NOT NULL)
- CreatedAt (DATETIME2, NOT NULL)

? **EventDate should NOT exist**

## After Fix

1. **Restart the application**
2. **Go to Admin Panel ? Events ? Create Event**
3. **Fill in the form and click Create Event**
4. **Event should be created successfully!**

## Need Help?

If you still face issues, check:
- SQL Server is running
- Database connection string is correct
- You have permissions to modify the database structure
