-- ========================================
-- Clear SMS Logs and Reset Balance
-- For Testing SMS Balance Deduction
-- ========================================

USE [WBS_Database] -- Change to your database name
GO

PRINT '?? WARNING: This will delete all SMS logs!'
PRINT 'Press Ctrl+C to cancel, or wait 5 seconds to continue...'
WAITFOR DELAY '00:00:05'
GO

-- Delete all SMS logs
DELETE FROM SmsLogs;
PRINT '? Deleted all SMS logs'
GO

-- Reset SMS Balance to 1000
UPDATE SmsBalances
SET AvailableBalance = 1000,
    LastUpdated = GETUTCDATE(),
    UpdatedBy = 'System Reset',
    Notes = 'Balance reset for testing - ' + CONVERT(VARCHAR, GETUTCDATE(), 120);

PRINT '? SMS Balance reset to 1000'
GO

-- Verify
SELECT 
    'Current SMS Balance' AS Info,
    AvailableBalance,
    LastUpdated,
    UpdatedBy
FROM SmsBalances;

SELECT 
    'Total SMS Logs' AS Info,
    COUNT(*) AS Count
FROM SmsLogs;

PRINT ''
PRINT '? SMS system reset complete!'
PRINT '?? You can now test SMS sending and balance will decrease properly'
PRINT '?? Check SMS Logs page to see real-time balance changes'
