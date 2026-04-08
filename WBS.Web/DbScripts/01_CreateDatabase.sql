-- =====================================================
-- WBS (Welfare Based Society) NGO Database Setup
-- Database Name: WBS_NGO
-- Description: Database for NGO/Charity website with 
--              Bengali (?????) language support
-- =====================================================

-- Drop database if exists (BE CAREFUL!)
-- Uncomment the following lines only if you want to recreate the database
/*
USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'WBS_NGO')
BEGIN
    ALTER DATABASE WBS_NGO SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE WBS_NGO;
    PRINT '? Existing database dropped';
END
GO
*/

-- Create new database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'WBS_NGO')
BEGIN
    CREATE DATABASE WBS_NGO
    COLLATE SQL_Latin1_General_CP1_CI_AS;
    PRINT '? Database WBS_NGO created successfully';
END
ELSE
BEGIN
    PRINT '? Database WBS_NGO already exists';
END
GO

-- Use the database
USE WBS_NGO;
GO

-- Enable Unicode support for Bengali text
-- Set database collation to support Unicode
ALTER DATABASE WBS_NGO 
COLLATE SQL_Latin1_General_CP1_CI_AS;
GO

PRINT '? Database setup completed successfully';
PRINT 'Database Name: WBS_NGO';
PRINT 'Server: (localdb)\mssqllocaldb';
PRINT 'Connection String: Server=(localdb)\mssqllocaldb;Database=WBS_NGO;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False';
GO
