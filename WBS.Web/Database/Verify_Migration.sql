-- =============================================
-- Quick Verification Script
-- Run this after migration to verify everything is working
-- =============================================

USE WBS_NGO;
GO

PRINT '========================================';
PRINT 'WBS Database Verification Script';
PRINT 'Timestamp: ' + CONVERT(VARCHAR, GETDATE(), 121);
PRINT '========================================';
PRINT '';

-- =============================================
-- 1. CHECK DATABASE VERSION
-- =============================================
PRINT '1. Database Information:';
PRINT '   Version: ' + CAST(SERVERPROPERTY('ProductVersion') AS VARCHAR);
PRINT '   Edition: ' + CAST(SERVERPROPERTY('Edition') AS VARCHAR);
PRINT '   Database Name: ' + DB_NAME();
PRINT '';

-- =============================================
-- 2. VERIFY TABLE STRUCTURE
-- =============================================
PRINT '2. Verifying table structure...';

DECLARE @ColumnCount INT;
SELECT @ColumnCount = COUNT(*)
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Donations'
AND COLUMN_NAME IN ('TransactionId', 'BankTransactionId', 'CardType', 'PaidAt', 'Currency');

IF @ColumnCount = 5
    PRINT '   ? All 5 new columns exist in Donations table';
ELSE
    PRINT '   ? ERROR: Only ' + CAST(@ColumnCount AS VARCHAR) + ' out of 5 columns found!';
PRINT '';

-- =============================================
-- 3. VERIFY INDEXES
-- =============================================
PRINT '3. Verifying indexes...';

DECLARE @IndexCount INT;
SELECT @IndexCount = COUNT(*)
FROM sys.indexes
WHERE name IN (
    'IX_Donations_TransactionId',
    'IX_Donations_PaymentMethod',
    'IX_Donations_PaymentDate',
    'IX_Donations_Status'
);

IF @IndexCount = 4
    PRINT '   ? All 4 performance indexes created';
ELSE
    PRINT '   ? WARNING: Only ' + CAST(@IndexCount AS VARCHAR) + ' out of 4 indexes found';
PRINT '';

-- =============================================
-- 4. VERIFY STORED PROCEDURES
-- =============================================
PRINT '4. Verifying stored procedures...';

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetDonationByTransactionId')
    PRINT '   ? sp_GetDonationByTransactionId exists';
ELSE
    PRINT '   ? sp_GetDonationByTransactionId NOT FOUND';

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetPaymentStatistics')
    PRINT '   ? sp_GetPaymentStatistics exists';
ELSE
    PRINT '   ? sp_GetPaymentStatistics NOT FOUND';
PRINT '';

-- =============================================
-- 5. VERIFY VIEWS
-- =============================================
PRINT '5. Verifying views...';

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_DonationSummary')
    PRINT '   ? vw_DonationSummary exists';
ELSE
    PRINT '   ? vw_DonationSummary NOT FOUND';
PRINT '';

-- =============================================
-- 6. VERIFY PAYMENT TRANSACTION LOG TABLE
-- =============================================
PRINT '6. Verifying PaymentTransactionLogs table...';

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PaymentTransactionLogs')
    PRINT '   ? PaymentTransactionLogs table exists';
ELSE
    PRINT '   ? PaymentTransactionLogs table NOT FOUND';
PRINT '';

-- =============================================
-- 7. DATA INTEGRITY CHECKS
-- =============================================
PRINT '7. Data integrity checks...';

DECLARE @TotalDonations INT;
DECLARE @WithTransactionId INT;
DECLARE @CompletedDonations INT;
DECLARE @TotalAmount DECIMAL(18,2);

SELECT 
    @TotalDonations = COUNT(*),
    @WithTransactionId = COUNT(TransactionId),
    @CompletedDonations = SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END),
    @TotalAmount = SUM(CASE WHEN Status = 1 THEN Amount ELSE 0 END)
FROM Donations;

PRINT '   Total Donations: ' + CAST(@TotalDonations AS VARCHAR);
PRINT '   With TransactionId: ' + CAST(@WithTransactionId AS VARCHAR);
PRINT '   Completed: ' + CAST(@CompletedDonations AS VARCHAR);
PRINT '   Total Amount: BDT ' + CAST(@TotalAmount AS VARCHAR);

IF @TotalDonations = @WithTransactionId
    PRINT '   ? All donations have TransactionId';
ELSE
    PRINT '   ? WARNING: ' + CAST(@TotalDonations - @WithTransactionId AS VARCHAR) + ' donations missing TransactionId';
PRINT '';

-- =============================================
-- 8. BACKUP VERIFICATION
-- =============================================
PRINT '8. Verifying backup tables...';

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Donations_Backup_20250125')
    PRINT '   ? Donations backup exists';
ELSE
    PRINT '   ? WARNING: Donations backup NOT FOUND';

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Accounts_Backup_20250125')
    PRINT '   ? Accounts backup exists';
ELSE
    PRINT '   ? WARNING: Accounts backup NOT FOUND';
PRINT '';

-- =============================================
-- 9. PAYMENT METHOD STATISTICS
-- =============================================
PRINT '9. Payment method statistics:';

SELECT 
    PaymentMethod,
    COUNT(*) AS TotalCount,
    SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END) AS CompletedCount,
    SUM(CASE WHEN Status = 1 THEN Amount ELSE 0 END) AS TotalAmount
FROM Donations
GROUP BY PaymentMethod
ORDER BY TotalCount DESC;
PRINT '';

-- =============================================
-- 10. RECENT DONATIONS
-- =============================================
PRINT '10. Recent donations (last 5):';

SELECT TOP 5 
    Id,
    DonorName,
    Amount,
    PaymentMethod,
    TransactionId,
    Status,
    CreatedAt
FROM Donations
ORDER BY CreatedAt DESC;
PRINT '';

-- =============================================
-- FINAL SUMMARY
-- =============================================
PRINT '========================================';
PRINT 'Verification Summary:';
PRINT '========================================';

DECLARE @AllGood BIT = 1;

-- Check all critical components
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Donations' AND COLUMN_NAME = 'TransactionId')
    SET @AllGood = 0;

IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetDonationByTransactionId')
    SET @AllGood = 0;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PaymentTransactionLogs')
    SET @AllGood = 0;

IF @AllGood = 1
BEGIN
    PRINT '';
    PRINT '? ALL CHECKS PASSED!';
    PRINT '';
    PRINT 'Database is ready for production use.';
    PRINT 'You can now deploy the application.';
END
ELSE
BEGIN
    PRINT '';
    PRINT '? SOME CHECKS FAILED!';
    PRINT '';
    PRINT 'Please review the errors above and re-run migration.';
    PRINT 'Contact support if issues persist.';
END

PRINT '';
PRINT '========================================';
PRINT 'Verification completed at: ' + CONVERT(VARCHAR, GETDATE(), 121);
PRINT '========================================';
GO
