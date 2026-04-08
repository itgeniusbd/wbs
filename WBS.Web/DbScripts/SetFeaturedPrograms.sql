-- Set the first 3 programs as featured for testing
UPDATE SDGPrograms
SET IsFeatured = 1
WHERE Id IN (SELECT TOP 3 Id FROM SDGPrograms WHERE IsActive = 1 ORDER BY DisplayOrder);

-- Verify the update
SELECT 
    Id,
    Title,
    TitleBn,
    IsFeatured,
    IsActive,
    DisplayOrder
FROM SDGPrograms
WHERE IsFeatured = 1;
