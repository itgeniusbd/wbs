-- Add EventId column to Volunteers table
-- This allows volunteers to select an event they're interested in

-- Check if column exists before adding
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Volunteers' AND COLUMN_NAME = 'EventId'
)
BEGIN
    ALTER TABLE [dbo].[Volunteers]
    ADD [EventId] INT NULL;

    -- Add foreign key constraint
    ALTER TABLE [dbo].[Volunteers]
    ADD CONSTRAINT [FK_Volunteers_Events_EventId] 
    FOREIGN KEY ([EventId]) REFERENCES [dbo].[Events] ([Id]);

    PRINT 'EventId column added to Volunteers table successfully.';
END
ELSE
BEGIN
    PRINT 'EventId column already exists in Volunteers table.';
END
GO
