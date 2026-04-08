-- First, check if IsFeatured column exists
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[SDGPrograms]') 
               AND name = 'IsFeatured')
BEGIN
    -- Add IsFeatured column if it doesn't exist
    ALTER TABLE [dbo].[SDGPrograms]
    ADD [IsFeatured] BIT NOT NULL DEFAULT 0;
    
    PRINT 'IsFeatured column added successfully!';
END
ELSE
BEGIN
    PRINT 'IsFeatured column already exists.';
END
GO

-- Set the first 3 active programs as featured
UPDATE TOP (3) SDGPrograms
SET IsFeatured = 1
WHERE IsActive = 1;

-- Show results
SELECT 
    Id,
    Title,
    TitleBn,
    IsFeatured,
    IsActive,
    DisplayOrder,
    SDGId
FROM SDGPrograms
WHERE IsActive = 1
ORDER BY IsFeatured DESC, DisplayOrder;

PRINT '';
PRINT '===================================';
PRINT 'Featured Programs Summary:';
PRINT '===================================';

SELECT 
    'Total Programs' AS Category,
    COUNT(*) AS Count
FROM SDGPrograms
UNION ALL
SELECT 
    'Active Programs' AS Category,
    COUNT(*) AS Count
FROM SDGPrograms
WHERE IsActive = 1
UNION ALL
SELECT 
    'Featured Programs' AS Category,
    COUNT(*) AS Count
FROM SDGPrograms
WHERE IsFeatured = 1 AND IsActive = 1;
