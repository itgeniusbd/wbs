-- =============================================
-- Quick Database Update Script
-- Run this in SQL Server Management Studio
-- =============================================

USE [YourDatabaseName]; -- Replace with your actual database name
GO

-- Add SDGId and ProgramId columns to Donations table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Donations' AND COLUMN_NAME = 'SDGId')
BEGIN
    PRINT 'Adding SDGId column to Donations table...';
    ALTER TABLE Donations ADD SDGId INT NULL;
    
    PRINT 'Adding foreign key constraint for SDGId...';
    ALTER TABLE Donations
    ADD CONSTRAINT FK_Donations_SDGs_SDGId
    FOREIGN KEY (SDGId) REFERENCES SDGs(Id);
    
    PRINT 'Creating index for SDGId...';
    CREATE INDEX IX_Donations_SDGId ON Donations(SDGId);
    
    PRINT 'SDGId column added successfully!';
END
ELSE
BEGIN
    PRINT 'SDGId column already exists.';
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Donations' AND COLUMN_NAME = 'ProgramId')
BEGIN
    PRINT 'Adding ProgramId column to Donations table...';
    ALTER TABLE Donations ADD ProgramId INT NULL;
    
    PRINT 'Adding foreign key constraint for ProgramId...';
    ALTER TABLE Donations
    ADD CONSTRAINT FK_Donations_SDGPrograms_ProgramId
    FOREIGN KEY (ProgramId) REFERENCES SDGPrograms(Id);
    
    PRINT 'Creating index for ProgramId...';
    CREATE INDEX IX_Donations_ProgramId ON Donations(ProgramId);
    
    PRINT 'ProgramId column added successfully!';
END
ELSE
BEGIN
    PRINT 'ProgramId column already exists.';
END
GO

-- Verify the changes
PRINT '==============================================';
PRINT 'Verification:';
PRINT '==============================================';

SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Donations' 
AND COLUMN_NAME IN ('SDGId', 'ProgramId');

PRINT '==============================================';
PRINT 'Migration completed successfully!';
PRINT 'The Donations table now has SDGId and ProgramId columns.';
PRINT '==============================================';
