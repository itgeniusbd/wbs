-- ============================================
-- Update ProgramExpenses Table - Change EventId to ProjectId
-- Run this in SQL Server Management Studio
-- ============================================

USE [WBS_NGO]  -- Your database name
GO

PRINT '=========================================='
PRINT 'Starting ProgramExpenses Table Update...'
PRINT '=========================================='

-- Step 1: Drop existing foreign key constraint for EventId (if exists)
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProgramExpenses_Events_EventId')
BEGIN
    ALTER TABLE [dbo].[ProgramExpenses] DROP CONSTRAINT [FK_ProgramExpenses_Events_EventId]
    PRINT '? Dropped FK_ProgramExpenses_Events_EventId'
END

-- Step 2: Drop existing index for EventId (if exists)
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProgramExpenses_EventId' AND object_id = OBJECT_ID('ProgramExpenses'))
BEGIN
    DROP INDEX [IX_ProgramExpenses_EventId] ON [dbo].[ProgramExpenses]
    PRINT '? Dropped IX_ProgramExpenses_EventId'
END

-- Step 3: Rename EventId column to ProjectId (if EventId exists)
IF EXISTS (SELECT * FROM sys.columns WHERE name = 'EventId' AND object_id = OBJECT_ID('ProgramExpenses'))
BEGIN
    EXEC sp_rename 'ProgramExpenses.EventId', 'ProjectId', 'COLUMN'
    PRINT '? Renamed EventId to ProjectId'
END
ELSE
BEGIN
    -- If EventId doesn't exist, check if ProjectId already exists
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = 'ProjectId' AND object_id = OBJECT_ID('ProgramExpenses'))
    BEGIN
        -- Add ProjectId column if neither exists
        ALTER TABLE [dbo].[ProgramExpenses] ADD [ProjectId] [int] NULL
        PRINT '? Added ProjectId column'
    END
    ELSE
    BEGIN
        PRINT '? ProjectId column already exists'
    END
END
GO

-- Step 4: Create new index for ProjectId
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProgramExpenses_ProjectId' AND object_id = OBJECT_ID('ProgramExpenses'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProgramExpenses_ProjectId] ON [dbo].[ProgramExpenses]
    (
        [ProjectId] ASC
    )
    PRINT '? Created IX_ProgramExpenses_ProjectId'
END
GO

-- Step 5: Add new foreign key constraint to SDGProjects
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProgramExpenses_SDGProjects_ProjectId')
BEGIN
    ALTER TABLE [dbo].[ProgramExpenses] WITH CHECK ADD CONSTRAINT [FK_ProgramExpenses_SDGProjects_ProjectId] 
    FOREIGN KEY([ProjectId]) REFERENCES [dbo].[SDGProjects] ([Id])
    
    ALTER TABLE [dbo].[ProgramExpenses] CHECK CONSTRAINT [FK_ProgramExpenses_SDGProjects_ProjectId]
    PRINT '? Created FK_ProgramExpenses_SDGProjects_ProjectId'
END
GO

-- Verify the changes
PRINT ''
PRINT '=========================================='
PRINT 'Verification:'
PRINT '=========================================='

-- Check if ProjectId column exists
IF EXISTS (SELECT * FROM sys.columns WHERE name = 'ProjectId' AND object_id = OBJECT_ID('ProgramExpenses'))
BEGIN
    PRINT '? ProjectId column exists'
END
ELSE
BEGIN
    PRINT '? ProjectId column NOT found'
END

-- Check if EventId column still exists
IF EXISTS (SELECT * FROM sys.columns WHERE name = 'EventId' AND object_id = OBJECT_ID('ProgramExpenses'))
BEGIN
    PRINT '? EventId column still exists (should be renamed)'
END
ELSE
BEGIN
    PRINT '? EventId column removed (renamed to ProjectId)'
END

-- Check foreign key
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProgramExpenses_SDGProjects_ProjectId')
BEGIN
    PRINT '? Foreign key to SDGProjects exists'
END
ELSE
BEGIN
    PRINT '? Foreign key to SDGProjects NOT found'
END

PRINT ''
PRINT '=========================================='
PRINT 'Update completed successfully!'
PRINT '=========================================='

-- Show table structure
SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.max_length AS MaxLength,
    c.is_nullable AS IsNullable
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID('ProgramExpenses')
ORDER BY c.column_id
GO
