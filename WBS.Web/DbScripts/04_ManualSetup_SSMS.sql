-- =======================================================
-- WBS_NGO Database - Complete Setup Script
-- Run this entire script in SSMS on your server
-- Server: DESKTOP-JUNKIQI\LA_SATTAR-PC
-- =======================================================

USE master;
GO

-- Drop existing database if needed (UNCOMMENT if you want fresh start)
/*
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'WBS_NGO')
BEGIN
    ALTER DATABASE WBS_NGO SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE WBS_NGO;
    PRINT '? Old database dropped';
END
GO
*/

-- Create database if not exists
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'WBS_NGO')
BEGIN
    CREATE DATABASE WBS_NGO;
    PRINT '? Database WBS_NGO created';
END
ELSE
BEGIN
    PRINT '? Database WBS_NGO already exists';
END
GO

USE WBS_NGO;
GO

PRINT '========================================='
PRINT 'Current Database: ' + DB_NAME()
PRINT '========================================='
PRINT ''

-- Check if tables exist
DECLARE @TableCount INT
SELECT @TableCount = COUNT(*) 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'

PRINT 'Current Tables: ' + CAST(@TableCount AS VARCHAR)
PRINT ''

IF @TableCount = 0
BEGIN
    PRINT '? No tables found!'
    PRINT ''
    PRINT '?? Next Steps:'
    PRINT '1. Update appsettings.json connection string:'
    PRINT '   "Server=DESKTOP-JUNKIQI\\LA_SATTAR-PC;Database=WBS_NGO;..."'
    PRINT ''
    PRINT '2. Update appsettings.Development.json with same connection string'
    PRINT ''
    PRINT '3. Run in Visual Studio Package Manager Console:'
    PRINT '   Update-Database'
    PRINT ''
    PRINT '   OR in terminal:'
    PRINT '   dotnet ef database update'
    PRINT ''
END
ELSE
BEGIN
    PRINT '? Tables exist! Showing list...'
    PRINT ''
    
    SELECT 
        ROW_NUMBER() OVER (ORDER BY TABLE_NAME) AS [#],
        TABLE_NAME AS [Table Name],
        (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = t.TABLE_NAME) AS [Columns]
    FROM INFORMATION_SCHEMA.TABLES t
    WHERE TABLE_TYPE = 'BASE TABLE'
    ORDER BY TABLE_NAME;
END
GO
