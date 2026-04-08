-- Check EventRegistrations table and data

USE [WBS_NGO]
GO

-- Check if EventRegistrations table exists
IF EXISTS (SELECT * FROM sysobjects WHERE name='EventRegistrations' AND xtype='U')
BEGIN
    PRINT '✓ EventRegistrations table exists'
    
    -- Show table structure
    PRINT ''
    PRINT '=== EventRegistrations Table Structure ==='
    SELECT 
        COLUMN_NAME as 'Column Name', 
        DATA_TYPE as 'Data Type', 
        IS_NULLABLE as 'Nullable'
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'EventRegistrations'
    ORDER BY ORDINAL_POSITION;
    
    -- Count total registrations
    DECLARE @TotalCount INT
    SELECT @TotalCount = COUNT(*) FROM EventRegistrations
    PRINT ''
    PRINT '=== Total Registrations Count ==='
    PRINT 'Total: ' + CAST(@TotalCount AS NVARCHAR)
    
    -- Show registrations by event
    PRINT ''
    PRINT '=== Registrations by Event ==='
    SELECT 
        e.Id as EventId,
        e.Title as EventTitle,
        COUNT(er.Id) as TotalRegistrations,
        SUM(CASE WHEN er.Status = 'Confirmed' THEN 1 ELSE 0 END) as ConfirmedCount,
        SUM(CASE WHEN er.Status = 'Pending' THEN 1 ELSE 0 END) as PendingCount,
        SUM(CASE WHEN er.Status = 'Cancelled' THEN 1 ELSE 0 END) as CancelledCount
    FROM Events e
    LEFT JOIN EventRegistrations er ON e.Id = er.EventId
    GROUP BY e.Id, e.Title
    ORDER BY e.Id;
    
    -- Show recent registrations
    PRINT ''
    PRINT '=== Recent Registrations (Last 10) ==='
    SELECT TOP 10
        er.Id,
        er.EventId,
        e.Title as EventTitle,
        er.FullName,
        er.Email,
        er.Status,
        er.RegisteredAt
    FROM EventRegistrations er
    INNER JOIN Events e ON er.EventId = e.Id
    ORDER BY er.RegisteredAt DESC;
END
ELSE
BEGIN
    PRINT '✗ EventRegistrations table does not exist!'
    PRINT 'Creating EventRegistrations table...'
    
    CREATE TABLE [dbo].[EventRegistrations] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [EventId] INT NOT NULL,
        [FullName] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(100) NOT NULL,
        [Phone] NVARCHAR(20) NOT NULL,
        [Address] NVARCHAR(200) NULL,
        [Organization] NVARCHAR(100) NULL,
        [AmountPaid] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [PaymentMethod] NVARCHAR(50) NOT NULL,
        [TransactionId] NVARCHAR(100) NULL,
        [Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',
        [Notes] NVARCHAR(MAX) NULL,
        [RegisteredAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [ConfirmedAt] DATETIME2 NULL,
        CONSTRAINT [FK_EventRegistrations_Events] FOREIGN KEY ([EventId]) 
            REFERENCES [Events]([Id]) ON DELETE CASCADE
    );
    
    PRINT '✓ EventRegistrations table created successfully!'
END
GO
