-- ============================================
-- Program Expenses Table Creation Script
-- Run this in SQL Server Management Studio
-- ============================================

USE [YourDatabaseName]  -- Replace with your actual database name
GO

-- Create ProgramExpenses Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProgramExpenses]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ProgramExpenses](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [SDGId] [int] NOT NULL,
        [ProgramId] [int] NOT NULL,
        [ProjectId] [int] NULL,
        [Amount] [decimal](18, 2) NOT NULL,
        [AccountId] [int] NOT NULL,
        [ExpenseDate] [datetime2](7) NOT NULL,
        [Details] [nvarchar](1000) NOT NULL,
        [IsActive] [bit] NOT NULL,
        [CreatedBy] [nvarchar](100) NULL,
        [CreatedAt] [datetime2](7) NOT NULL,
        [UpdatedAt] [datetime2](7) NULL,
     CONSTRAINT [PK_ProgramExpenses] PRIMARY KEY CLUSTERED ([Id] ASC)
    ) ON [PRIMARY]

    PRINT 'Table ProgramExpenses created successfully'
END
ELSE
BEGIN
    PRINT 'Table ProgramExpenses already exists'
END
GO

-- Create Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProgramExpenses_SDGId' AND object_id = OBJECT_ID('ProgramExpenses'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProgramExpenses_SDGId] ON [dbo].[ProgramExpenses]
    (
        [SDGId] ASC
    )
    PRINT 'Index IX_ProgramExpenses_SDGId created'
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProgramExpenses_ProgramId' AND object_id = OBJECT_ID('ProgramExpenses'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProgramExpenses_ProgramId] ON [dbo].[ProgramExpenses]
    (
        [ProgramId] ASC
    )
    PRINT 'Index IX_ProgramExpenses_ProgramId created'
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProgramExpenses_ProjectId' AND object_id = OBJECT_ID('ProgramExpenses'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProgramExpenses_ProjectId] ON [dbo].[ProgramExpenses]
    (
        [ProjectId] ASC
    )
    PRINT 'Index IX_ProgramExpenses_ProjectId created'
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProgramExpenses_AccountId' AND object_id = OBJECT_ID('ProgramExpenses'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProgramExpenses_AccountId] ON [dbo].[ProgramExpenses]
    (
        [AccountId] ASC
    )
    PRINT 'Index IX_ProgramExpenses_AccountId created'
END
GO

-- Add Foreign Key Constraints
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProgramExpenses_SDGs_SDGId')
BEGIN
    ALTER TABLE [dbo].[ProgramExpenses] WITH CHECK ADD CONSTRAINT [FK_ProgramExpenses_SDGs_SDGId] 
    FOREIGN KEY([SDGId]) REFERENCES [dbo].[SDGs] ([Id])
    
    ALTER TABLE [dbo].[ProgramExpenses] CHECK CONSTRAINT [FK_ProgramExpenses_SDGs_SDGId]
    PRINT 'Foreign Key FK_ProgramExpenses_SDGs_SDGId created'
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProgramExpenses_SDGPrograms_ProgramId')
BEGIN
    ALTER TABLE [dbo].[ProgramExpenses] WITH CHECK ADD CONSTRAINT [FK_ProgramExpenses_SDGPrograms_ProgramId] 
    FOREIGN KEY([ProgramId]) REFERENCES [dbo].[SDGPrograms] ([Id])
    
    ALTER TABLE [dbo].[ProgramExpenses] CHECK CONSTRAINT [FK_ProgramExpenses_SDGPrograms_ProgramId]
    PRINT 'Foreign Key FK_ProgramExpenses_SDGPrograms_ProgramId created'
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProgramExpenses_SDGProjects_ProjectId')
BEGIN
    ALTER TABLE [dbo].[ProgramExpenses] WITH CHECK ADD CONSTRAINT [FK_ProgramExpenses_SDGProjects_ProjectId] 
    FOREIGN KEY([ProjectId]) REFERENCES [dbo].[SDGProjects] ([Id])
    
    ALTER TABLE [dbo].[ProgramExpenses] CHECK CONSTRAINT [FK_ProgramExpenses_SDGProjects_ProjectId]
    PRINT 'Foreign Key FK_ProgramExpenses_SDGProjects_ProjectId created'
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProgramExpenses_Accounts_AccountId')
BEGIN
    ALTER TABLE [dbo].[ProgramExpenses] WITH CHECK ADD CONSTRAINT [FK_ProgramExpenses_Accounts_AccountId] 
    FOREIGN KEY([AccountId]) REFERENCES [dbo].[Accounts] ([Id])
    
    ALTER TABLE [dbo].[ProgramExpenses] CHECK CONSTRAINT [FK_ProgramExpenses_Accounts_AccountId]
    PRINT 'Foreign Key FK_ProgramExpenses_Accounts_AccountId created'
END
GO

-- Verify table creation
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProgramExpenses]') AND type in (N'U'))
BEGIN
    PRINT '============================================'
    PRINT 'SUCCESS: ProgramExpenses table is ready!'
    PRINT '============================================'
    
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
END
GO
