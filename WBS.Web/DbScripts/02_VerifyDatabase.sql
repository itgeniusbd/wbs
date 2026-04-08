-- =====================================================
-- WBS_NGO Database Verification Script
-- Check database structure and data
-- =====================================================

USE WBS_NGO;
GO

PRINT '========================================='
PRINT 'WBS_NGO Database Verification Report'
PRINT '========================================='
PRINT ''

-- Check database exists
PRINT '1. Database Information:'
PRINT '   Name: ' + DB_NAME()
PRINT '   Collation: ' + CAST(DATABASEPROPERTYEX(DB_NAME(), 'Collation') AS VARCHAR(100))
PRINT '   Status: ' + CAST(DATABASEPROPERTYEX(DB_NAME(), 'Status') AS VARCHAR(50))
PRINT ''

-- List all tables
PRINT '2. Database Tables:'
SELECT 
    ROW_NUMBER() OVER (ORDER BY TABLE_NAME) AS [#],
    TABLE_NAME AS [Table Name],
    (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = t.TABLE_NAME) AS [Columns]
FROM INFORMATION_SCHEMA.TABLES t
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
PRINT ''

-- Check Identity tables
PRINT '3. Identity System Status:'
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AspNetUsers')
BEGIN
    DECLARE @UserCount INT
    SELECT @UserCount = COUNT(*) FROM AspNetUsers
    PRINT '   ? AspNetUsers table exists'
    PRINT '   Users registered: ' + CAST(@UserCount AS VARCHAR)
END
ELSE
    PRINT '   ? AspNetUsers table missing!'
PRINT ''

-- Check seeded data
PRINT '4. Seed Data Status:'

-- Donation Types
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DonationTypes')
BEGIN
    DECLARE @DonationTypeCount INT
    SELECT @DonationTypeCount = COUNT(*) FROM DonationTypes
    PRINT '   DonationTypes: ' + CAST(@DonationTypeCount AS VARCHAR) + ' records'
    
    IF @DonationTypeCount > 0
    BEGIN
        PRINT '   Donation Types:'
        SELECT '     - ' + Name + ' (' + ISNULL(NameBn, 'N/A') + ')' AS [Type]
        FROM DonationTypes
        ORDER BY DisplayOrder
    END
END

-- SDGs
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SDGs')
BEGIN
    DECLARE @SDGCount INT
    SELECT @SDGCount = COUNT(*) FROM SDGs
    PRINT '   SDGs: ' + CAST(@SDGCount AS VARCHAR) + ' records'
    
    IF @SDGCount > 0
    BEGIN
        PRINT '   SDG Goals:'
        SELECT '     - SDG ' + CAST(Number AS VARCHAR) + ': ' + Name + ' (' + ISNULL(NameBn, 'N/A') + ')' AS [Goal]
        FROM SDGs
        ORDER BY Number
    END
END
PRINT ''

-- Check for Bengali text support
PRINT '5. Unicode/Bengali Support:'
IF EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE DATA_TYPE = 'nvarchar' 
    AND TABLE_NAME = 'Sliders' 
    AND COLUMN_NAME = 'TitleBn'
)
    PRINT '   ? NVARCHAR columns configured for Bengali text'
ELSE
    PRINT '   ? Bengali text columns might have issues'
PRINT ''

-- Table row counts
PRINT '6. Content Statistics:'
SELECT 
    t.TABLE_NAME AS [Table],
    p.rows AS [Row Count]
FROM INFORMATION_SCHEMA.TABLES t
INNER JOIN sys.tables st ON t.TABLE_NAME = st.name
INNER JOIN sys.partitions p ON st.object_id = p.object_id
WHERE t.TABLE_TYPE = 'BASE TABLE'
    AND p.index_id IN (0,1)
    AND p.rows > 0
ORDER BY p.rows DESC;
PRINT ''

-- Check admin user
PRINT '7. Admin User Status:'
IF EXISTS (SELECT * FROM AspNetUsers WHERE Email = 'admin@wbs.org')
BEGIN
    PRINT '   ? Admin user exists (admin@wbs.org)'
    
    IF EXISTS (
        SELECT * FROM AspNetUsers u
        INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
        INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
        WHERE u.Email = 'admin@wbs.org' AND r.Name = 'Admin'
    )
        PRINT '   ? Admin role assigned'
    ELSE
        PRINT '   ? Admin role NOT assigned!'
END
ELSE
    PRINT '   ? Admin user not found!'
PRINT ''

PRINT '========================================='
PRINT 'Verification Complete!'
PRINT '========================================='
GO
