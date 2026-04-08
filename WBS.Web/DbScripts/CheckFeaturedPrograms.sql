-- Check if IsFeatured column exists and check featured programs
SELECT 
    Id,
    Title,
    TitleBn,
    IsFeatured,
    IsActive,
    DisplayOrder,
    SDGId
FROM SDGPrograms
ORDER BY IsFeatured DESC, DisplayOrder;

-- Count of featured programs
SELECT 
    COUNT(*) as TotalPrograms,
    SUM(CASE WHEN IsFeatured = 1 THEN 1 ELSE 0 END) as FeaturedPrograms,
    SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END) as ActivePrograms
FROM SDGPrograms;
