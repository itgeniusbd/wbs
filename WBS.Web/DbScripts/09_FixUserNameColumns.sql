-- Fix FirstName and LastName to be NOT NULL
USE WBS_NGO;
GO

PRINT 'Fixing FirstName and LastName columns...'

-- Update NULL values to empty string
UPDATE AspNetUsers SET FirstName = '' WHERE FirstName IS NULL;
UPDATE AspNetUsers SET LastName = '' WHERE LastName IS NULL;

-- Alter columns to NOT NULL
ALTER TABLE AspNetUsers ALTER COLUMN FirstName NVARCHAR(100) NOT NULL;
ALTER TABLE AspNetUsers ALTER COLUMN LastName NVARCHAR(100) NOT NULL;

PRINT '? Columns fixed successfully!'
GO
