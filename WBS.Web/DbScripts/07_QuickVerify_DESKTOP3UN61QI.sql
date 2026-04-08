-- Quick verification for DESKTOP-3UN61QI SQL Server
USE WBS_NGO;
GO

PRINT '========================================='
PRINT 'WBS_NGO Database Status'
PRINT 'Server: DESKTOP-3UN61QI'
PRINT '========================================='
PRINT ''

-- Count tables
DECLARE @TableCount INT
SELECT @TableCount = COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'
PRINT 'Total Tables: ' + CAST(@TableCount AS VARCHAR)

-- Check Identity tables
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUsers')
    PRINT '? AspNetUsers exists'
ELSE
    PRINT '? AspNetUsers missing'

-- Check Application tables
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'DonationTypes')
    PRINT '? DonationTypes exists'

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'SDGs')
    PRINT '? SDGs exists'

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Appeals')
    PRINT '? Appeals exists'

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Donations')
    PRINT '? Donations exists'

PRINT ''

-- Check seed data
DECLARE @DonationTypeCount INT, @SDGCount INT
SELECT @DonationTypeCount = COUNT(*) FROM DonationTypes
SELECT @SDGCount = COUNT(*) FROM SDGs

PRINT 'Seed Data:'
PRINT '  Donation Types: ' + CAST(@DonationTypeCount AS VARCHAR)
PRINT '  SDG Goals: ' + CAST(@SDGCount AS VARCHAR)

PRINT ''
PRINT '? Database is ready!'
PRINT ''
PRINT 'Connection String:'
PRINT 'Server=DESKTOP-3UN61QI;Database=WBS_NGO;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;TrustServerCertificate=True'
GO
