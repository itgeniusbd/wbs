-- =============================================
-- WBS Bangladesh - Migration Rollback Script
-- Date: January 25, 2025
-- Version: 1.0.0
-- ?? USE WITH CAUTION - THIS WILL UNDO THE MIGRATION
-- =============================================

USE WBS_NGO;
GO

PRINT '========================================';
PRINT 'Starting Migration Rollback';
PRINT 'Timestamp: ' + CONVERT(VARCHAR, GETDATE(), 121);
PRINT '========================================';
GO

-- Ask for confirmation
DECLARE @Confirm VARCHAR(10);
SET @Confirm = 'NO'; -- Change to 'YES' to proceed

IF @Confirm <> 'YES'
BEGIN
    PRINT '';
    PRINT '?? ROLLBACK CANCELLED';
    PRINT 'To proceed with rollback, change @Confirm to ''YES'' in the script';
    PRINT '';
    RETURN;
END
GO

-- =============================================
-- SECTION 1: DROP NEW OBJECTS
-- =============================================
PRINT '';
PRINT 'SECTION 1: Dropping new objects...';
GO

-- Drop stored procedures
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetDonationByTransactionId')
BEGIN
    DROP PROCEDURE sp_GetDonationByTransactionId;
    PRINT '? Dropped sp_GetDonationByTransactionId';
END
GO

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetPaymentStatistics')
BEGIN
    DROP PROCEDURE sp_GetPaymentStatistics;
    PRINT '? Dropped sp_GetPaymentStatistics';
END
GO

-- Drop views
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_DonationSummary')
BEGIN
    DROP VIEW vw_DonationSummary;
    PRINT '? Dropped vw_DonationSummary';
END
GO

-- Drop PaymentTransactionLogs table
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PaymentTransactionLogs')
BEGIN
    DROP TABLE PaymentTransactionLogs;
    PRINT '? Dropped PaymentTransactionLogs table';
END
GO

-- =============================================
-- SECTION 2: DROP INDEXES
-- =============================================
PRINT '';
PRINT 'SECTION 2: Dropping indexes...';
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Donations_TransactionId')
BEGIN
    DROP INDEX IX_Donations_TransactionId ON Donations;
    PRINT '? Dropped index IX_Donations_TransactionId';
END
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Donations_PaymentMethod')
BEGIN
    DROP INDEX IX_Donations_PaymentMethod ON Donations;
    PRINT '? Dropped index IX_Donations_PaymentMethod';
END
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Donations_PaymentDate')
BEGIN
    DROP INDEX IX_Donations_PaymentDate ON Donations;
    PRINT '? Dropped index IX_Donations_PaymentDate';
END
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Donations_Status')
BEGIN
    DROP INDEX IX_Donations_Status ON Donations;
    PRINT '? Dropped index IX_Donations_Status';
END
GO

-- =============================================
-- SECTION 3: DROP NEW COLUMNS
-- =============================================
PRINT '';
PRINT 'SECTION 3: Dropping new columns...';
GO

-- Drop Currency column
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'Currency')
BEGIN
    ALTER TABLE Donations DROP COLUMN Currency;
    PRINT '? Dropped Currency column';
END
GO

-- Drop PaidAt column
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'PaidAt')
BEGIN
    ALTER TABLE Donations DROP COLUMN PaidAt;
    PRINT '? Dropped PaidAt column';
END
GO

-- Drop CardType column
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'CardType')
BEGIN
    ALTER TABLE Donations DROP COLUMN CardType;
    PRINT '? Dropped CardType column';
END
GO

-- Drop BankTransactionId column
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'BankTransactionId')
BEGIN
    ALTER TABLE Donations DROP COLUMN BankTransactionId;
    PRINT '? Dropped BankTransactionId column';
END
GO

-- Drop TransactionId column
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'TransactionId')
BEGIN
    ALTER TABLE Donations DROP COLUMN TransactionId;
    PRINT '? Dropped TransactionId column';
END
GO

-- =============================================
-- SECTION 4: RESTORE FROM BACKUP (OPTIONAL)
-- =============================================
PRINT '';
PRINT 'SECTION 4: Backup restoration (manual step)...';
PRINT '?? If you need to restore data, use the backup tables:';
PRINT '   - Donations_Backup_20250125';
PRINT '   - Accounts_Backup_20250125';
GO

-- Uncomment to restore from backup (?? THIS WILL OVERWRITE CURRENT DATA)
/*
TRUNCATE TABLE Donations;
INSERT INTO Donations SELECT * FROM Donations_Backup_20250125;
PRINT '? Restored Donations from backup';

TRUNCATE TABLE Accounts;
INSERT INTO Accounts SELECT * FROM Accounts_Backup_20250125;
PRINT '? Restored Accounts from backup';
*/

-- =============================================
-- FINAL MESSAGE
-- =============================================
PRINT '';
PRINT '========================================';
PRINT '? Rollback completed successfully!';
PRINT 'Timestamp: ' + CONVERT(VARCHAR, GETDATE(), 121);
PRINT '========================================';
PRINT '';
PRINT 'Note: Backup tables still exist:';
PRINT '  - Donations_Backup_20250125';
PRINT '  - Accounts_Backup_20250125';
PRINT '';
PRINT 'You can drop them manually if no longer needed.';
GO
