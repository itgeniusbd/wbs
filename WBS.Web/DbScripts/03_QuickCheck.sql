-- Quick Database Status Check
USE WBS_NGO;
GO

PRINT '? Database: WBS_NGO Connected!'
PRINT ''

-- Count tables
DECLARE @TableCount INT
SELECT @TableCount = COUNT(*) 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'

PRINT '?? Total Tables: ' + CAST(@TableCount AS VARCHAR)
PRINT ''

-- Show first 15 tables
PRINT '?? Tables Created:'
SELECT TOP 15 
    ROW_NUMBER() OVER (ORDER BY TABLE_NAME) AS [#],
    TABLE_NAME AS [Table Name]
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME
GO

-- Check admin user
IF EXISTS (SELECT * FROM AspNetUsers WHERE Email = 'admin@wbs.org')
    PRINT '?? ? Admin user exists: admin@wbs.org'
ELSE
    PRINT '?? ? Admin user will be created on app start'
GO

-- Check seed data
DECLARE @DonationTypes INT, @SDGs INT
SELECT @DonationTypes = COUNT(*) FROM DonationTypes
SELECT @SDGs = COUNT(*) FROM SDGs

PRINT ''
PRINT '?? Seed Data:'
PRINT '   - Donation Types: ' + CAST(@DonationTypes AS VARCHAR)
PRINT '   - SDG Goals: ' + CAST(@SDGs AS VARCHAR)
PRINT ''
PRINT '? Database is ready!'
PRINT ''
PRINT '?? Next: Run the application (dotnet run or F5)'
GO
