-- Check if Bangla content is stored correctly in the database
USE [WBS_NGO];
GO

-- Check the actual content stored in the Pages table
SELECT 
    Id,
    Title,
    TitleBn,
    CAST(LEFT(Content, 200) AS NVARCHAR(200)) AS ContentPreview,
    CAST(LEFT(ContentBn, 200) AS NVARCHAR(200)) AS ContentBnPreview,
    Slug,
    IsActive
FROM Pages
WHERE Slug IN ('privacy-policy', 'terms-conditions')
ORDER BY Slug;
GO

-- Check the column data types
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    COLLATION_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Pages'
    AND COLUMN_NAME IN ('Title', 'TitleBn', 'Content', 'ContentBn')
ORDER BY COLUMN_NAME;
GO
