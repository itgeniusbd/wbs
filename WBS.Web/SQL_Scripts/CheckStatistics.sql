-- Check if data was inserted successfully
USE [WbsDb];
GO

PRINT '=== Checking Database Statistics ===';
PRINT '';

-- Check Programs
SELECT 
    @ProgramCount = COUNT(*) 
FROM SDGPrograms 
WHERE IsActive = 1;

PRINT 'Total Active Programs: ' + CAST(ISNULL((SELECT COUNT(*) FROM SDGPrograms WHERE IsActive = 1), 0) AS VARCHAR);
PRINT '';

-- Check Projects/Events
PRINT 'Total Active Events: ' + CAST(ISNULL((SELECT COUNT(*) FROM SDGProjects WHERE IsActive = 1), 0) AS VARCHAR);
PRINT '';

-- Check Districts
PRINT 'Distinct Districts: ' + CAST(ISNULL((SELECT COUNT(DISTINCT District) FROM SDGProjects WHERE IsActive = 1 AND District IS NOT NULL AND District != ''), 0) AS VARCHAR);
PRINT '';

-- Check Thanas
PRINT 'Distinct Thanas: ' + CAST(ISNULL((SELECT COUNT(DISTINCT Thana) FROM SDGProjects WHERE IsActive = 1 AND Thana IS NOT NULL AND Thana != ''), 0) AS VARCHAR);
PRINT '';

-- Check Beneficiaries
PRINT 'Total Beneficiaries: ' + CAST(ISNULL((SELECT SUM(BeneficiaryCount) FROM SDGProjects WHERE IsActive = 1), 0) AS VARCHAR);
PRINT '';

-- Show all programs with their event counts
PRINT '=== Programs and their Events ===';
SELECT 
    p.Id,
    p.Title,
    p.IsActive,
    p.IsFeatured,
    EventCount = (SELECT COUNT(*) FROM SDGProjects WHERE SDGProgramId = p.Id AND IsActive = 1)
FROM SDGPrograms p
ORDER BY p.Id;

PRINT '';
PRINT '=== All Events/Projects ===';
SELECT 
    Id,
    Title,
    District,
    Thana,
    BeneficiaryCount,
    SDGProgramId,
    IsActive
FROM SDGProjects
WHERE IsActive = 1
ORDER BY SDGProgramId, Id;
