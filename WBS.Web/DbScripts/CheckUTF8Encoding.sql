-- Check Database Collation for UTF-8 Support
-- Run this script to verify and fix UTF-8 encoding issues

-- 1. Check current database collation
SELECT 
    name AS DatabaseName,
    collation_name AS Collation
FROM sys.databases
WHERE name = DB_NAME();

-- 2. Check table collations
SELECT 
    t.name AS TableName,
    c.name AS ColumnName,
    c.collation_name AS Collation,
    ty.name AS DataType
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
INNER JOIN sys.types ty ON c.user_type_id = ty.user_type_id
WHERE c.collation_name IS NOT NULL
  AND t.name IN ('SDGPrograms', 'SDGProjects')
ORDER BY t.name, c.name;

-- 3. Check sample data from SDGPrograms
SELECT 
    Id,
    Title,
    TitleBn,
    LEN(TitleBn) AS TitleBn_Length,
    CAST(TitleBn AS VARBINARY(MAX)) AS TitleBn_Binary
FROM SDGPrograms
WHERE TitleBn IS NOT NULL;

-- 4. Fix: Update database collation to support UTF-8 (if needed)
-- WARNING: Backup your database before running this!

/*
-- Uncomment to apply fix:

-- Change database collation
ALTER DATABASE [YourDatabaseName] 
COLLATE SQL_Latin1_General_CP1_CI_AS;

-- Or use Unicode collation
ALTER DATABASE [YourDatabaseName] 
COLLATE Latin1_General_100_CI_AS_SC_UTF8;
*/

-- 5. Fix individual columns if needed
/*
-- Update TitleBn column to NVARCHAR with proper collation
ALTER TABLE SDGPrograms 
ALTER COLUMN TitleBn NVARCHAR(200) 
COLLATE Latin1_General_100_CI_AS_SC_UTF8;

ALTER TABLE SDGPrograms 
ALTER COLUMN DescriptionBn NVARCHAR(MAX) 
COLLATE Latin1_General_100_CI_AS_SC_UTF8;
*/

-- 6. Verify if data is stored correctly
-- If you see '??????', the data is corrupted in database
-- You need to re-enter the Bangla text

SELECT 
    Id,
    Title,
    TitleBn,
    CASE 
        WHEN TitleBn LIKE '%?%' THEN 'CORRUPTED - Need to re-enter'
        WHEN TitleBn IS NULL THEN 'NULL'
        ELSE 'OK'
    END AS DataStatus
FROM SDGPrograms;

SELECT 
    Id,
    Title,
    TitleBn,
    CASE 
        WHEN TitleBn LIKE '%?%' THEN 'CORRUPTED - Need to re-enter'
        WHEN TitleBn IS NULL THEN 'NULL'
        ELSE 'OK'
    END AS DataStatus
FROM SDGProjects;
