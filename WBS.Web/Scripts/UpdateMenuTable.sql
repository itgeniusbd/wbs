-- Add missing columns to Menus table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Menus]') AND name = 'CreatedAt')
BEGIN
    ALTER TABLE [dbo].[Menus] ADD [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE();
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Menus]') AND name = 'CssClass')
BEGIN
    ALTER TABLE [dbo].[Menus] ADD [CssClass] nvarchar(max) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Menus]') AND name = 'IsExternal')
BEGIN
    ALTER TABLE [dbo].[Menus] ADD [IsExternal] bit NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Menus]') AND name = 'PageId')
BEGIN
    ALTER TABLE [dbo].[Menus] ADD [PageId] int NULL;
    
    -- Add foreign key constraint
    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Menus_Pages_PageId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Menus]'))
    BEGIN
        ALTER TABLE [dbo].[Menus] ADD CONSTRAINT [FK_Menus_Pages_PageId] FOREIGN KEY([PageId]) REFERENCES [dbo].[Pages]([Id]);
    END
    
    -- Create index
    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Menus]') AND name = 'IX_Menus_PageId')
    BEGIN
        CREATE INDEX [IX_Menus_PageId] ON [dbo].[Menus]([PageId]);
    END
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Menus]') AND name = 'UpdatedAt')
BEGIN
    ALTER TABLE [dbo].[Menus] ADD [UpdatedAt] datetime2 NULL;
END

PRINT 'Menu table updated successfully!';
