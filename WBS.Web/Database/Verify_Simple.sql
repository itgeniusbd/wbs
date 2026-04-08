-- =============================================
-- Simple Verification Script (FIXED)
-- =============================================

USE WBS_NGO;
GO

PRINT '========================================';
PRINT 'Database Verification (Simple Check)';
PRINT '========================================';
PRINT '';

-- Check if required columns exist
PRINT '1. Checking required columns...';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'TransactionId')
    PRINT '   ? TransactionId exists';
ELSE
    PRINT '   ? TransactionId MISSING';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'BankTransactionId')
    PRINT '   ? BankTransactionId exists';
ELSE
    PRINT '   ? BankTransactionId MISSING';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'CardType')
    PRINT '   ? CardType exists';
ELSE
    PRINT '   ? CardType MISSING';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'PaidAt')
    PRINT '   ? PaidAt exists';
ELSE
    PRINT '   ? PaidAt MISSING';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'Currency')
    PRINT '   ? Currency exists';
ELSE
    PRINT '   ? Currency MISSING';

PRINT '';

-- Check PaymentTransactionLogs table
PRINT '2. Checking PaymentTransactionLogs table...';
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PaymentTransactionLogs')
    PRINT '   ? PaymentTransactionLogs exists';
ELSE
    PRINT '   ? PaymentTransactionLogs MISSING';
PRINT '';

-- Check stored procedures
PRINT '3. Checking stored procedures...';
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetDonationByTransactionId')
    PRINT '   ? sp_GetDonationByTransactionId exists';
ELSE
    PRINT '   ? sp_GetDonationByTransactionId MISSING';

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetPaymentStatistics')
    PRINT '   ? sp_GetPaymentStatistics exists';
ELSE
    PRINT '   ? sp_GetPaymentStatistics MISSING';
PRINT '';

-- Check view
PRINT '4. Checking view...';
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_DonationSummary')
    PRINT '   ? vw_DonationSummary exists';
ELSE
    PRINT '   ? vw_DonationSummary MISSING';
PRINT '';

-- Data statistics
PRINT '5. Data statistics:';
DECLARE @Total INT, @WithTxnId INT, @Completed INT;

SELECT 
    @Total = COUNT(*),
    @WithTxnId = SUM(CASE WHEN TransactionId IS NOT NULL THEN 1 ELSE 0 END),
    @Completed = SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END)
FROM Donations;

PRINT '   Total Donations: ' + CAST(@Total AS VARCHAR);
PRINT '   With TransactionId: ' + CAST(@WithTxnId AS VARCHAR);
PRINT '   Completed: ' + CAST(@Completed AS VARCHAR);

PRINT '';
PRINT '========================================';
IF @Total = @WithTxnId
    PRINT '? ALL CHECKS PASSED!';
ELSE
    PRINT '?? SOME CHECKS FAILED - Review above';
PRINT '========================================';
GO
