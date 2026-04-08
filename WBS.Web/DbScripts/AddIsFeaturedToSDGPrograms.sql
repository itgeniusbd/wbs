-- Add IsFeatured column to SDGPrograms table
-- This allows programs to be marked as featured and shown on the home page

-- Add the IsFeatured column with default value false
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SDGPrograms]') AND name = 'IsFeatured')
BEGIN
    ALTER TABLE [dbo].[SDGPrograms]
    ADD [IsFeatured] BIT NOT NULL DEFAULT 0;
    
    PRINT 'IsFeatured column added to SDGPrograms table successfully';
END
ELSE
BEGIN
    PRINT 'IsFeatured column already exists in SDGPrograms table';
END
GO

-- Optional: Mark first 3 programs as featured for testing
-- UPDATE TOP(3) [dbo].[SDGPrograms]
-- SET [IsFeatured] = 1
-- WHERE [IsActive] = 1;
-- GO
