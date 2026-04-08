-- Insert sample event registrations for testing

USE [WBS_NGO]
GO

-- Check if we have any events
IF NOT EXISTS (SELECT * FROM Events)
BEGIN
    PRINT '? No events found. Please create an event first.'
    RETURN
END
GO

-- Get the first active event
DECLARE @EventId INT
SELECT TOP 1 @EventId = Id FROM Events WHERE IsActive = 1 ORDER BY StartDate

IF @EventId IS NULL
BEGIN
    PRINT '? No active events found.'
    RETURN
END
GO

-- Insert sample registrations if none exist for this event
DECLARE @EventId INT
SELECT TOP 1 @EventId = Id FROM Events WHERE IsActive = 1 ORDER BY StartDate

IF NOT EXISTS (SELECT * FROM EventRegistrations WHERE EventId = @EventId)
BEGIN
    PRINT 'Inserting sample registrations for Event ID: ' + CAST(@EventId AS NVARCHAR)
    
    INSERT INTO EventRegistrations (EventId, FullName, Email, Phone, Status, RegisteredAt, ConfirmedAt)
    VALUES 
    (@EventId, 'John Doe', 'john.doe@example.com', '01712345678', 'Confirmed', GETUTCDATE(), GETUTCDATE()),
    (@EventId, 'Jane Smith', 'jane.smith@example.com', '01787654321', 'Confirmed', GETUTCDATE(), GETUTCDATE()),
    (@EventId, 'Ahmed Khan', 'ahmed.khan@example.com', '01923456789', 'Confirmed', GETUTCDATE(), GETUTCDATE()),
    (@EventId, 'Fatima Rahman', 'fatima.rahman@example.com', '01634567890', 'Pending', GETUTCDATE(), NULL),
    (@EventId, 'Karim Hossain', 'karim.hossain@example.com', '01545678901', 'Confirmed', GETUTCDATE(), GETUTCDATE());
    
    PRINT '? 5 sample registrations inserted successfully!'
    
    -- Show summary
    SELECT 
        e.Title as EventTitle,
        COUNT(er.Id) as TotalRegistrations,
        SUM(CASE WHEN er.Status = 'Confirmed' THEN 1 ELSE 0 END) as Confirmed,
        SUM(CASE WHEN er.Status = 'Pending' THEN 1 ELSE 0 END) as Pending
    FROM Events e
    INNER JOIN EventRegistrations er ON e.Id = er.EventId
    WHERE e.Id = @EventId
    GROUP BY e.Title;
END
ELSE
BEGIN
    PRINT '? Registrations already exist for this event.'
    
    -- Show current registrations
    SELECT 
        e.Title as EventTitle,
        COUNT(er.Id) as TotalRegistrations,
        SUM(CASE WHEN er.Status = 'Confirmed' THEN 1 ELSE 0 END) as Confirmed,
        SUM(CASE WHEN er.Status = 'Pending' THEN 1 ELSE 0 END) as Pending
    FROM Events e
    INNER JOIN EventRegistrations er ON e.Id = er.EventId
    WHERE e.Id = @EventId
    GROUP BY e.Title;
END
GO
