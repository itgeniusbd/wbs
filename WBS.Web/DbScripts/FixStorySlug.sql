-- Fix existing story slugs
-- This script will clean up and fix slug formatting

USE [WBS_NGO]
GO

-- Update the slug to lowercase and replace spaces with hyphens
UPDATE [dbo].[Stories]
SET [Slug] = LOWER(REPLACE(REPLACE([Slug], ' ', '-'), '''', ''))
WHERE [Slug] LIKE '% %' OR [Slug] LIKE '%''%'
GO

-- If you have a specific story with issues, you can update it directly:
-- UPDATE [dbo].[Stories]
-- SET [Slug] = 'more-stories'
-- WHERE [Title] = 'Where to Find More Stories'
-- GO

-- Check all slugs
SELECT Id, Title, Slug FROM [dbo].[Stories]
GO

PRINT 'Story slugs have been updated!'
GO
