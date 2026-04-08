-- =============================================
-- WBS Bangladesh - Production Database Migration
-- SSLCommerz Payment Gateway Integration
-- Date: January 25, 2025
-- Version: 1.0.0
-- =============================================

USE WBS_NGO;
GO

PRINT '========================================';
PRINT 'Starting WBS Production Database Migration';
PRINT 'Timestamp: ' + CONVERT(VARCHAR, GETDATE(), 121);
PRINT '========================================';
GO

-- =============================================
-- SECTION 1: BACKUP EXISTING DATA
-- =============================================
PRINT '';
PRINT 'SECTION 1: Creating backup tables...';
GO

-- Backup Donations table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Donations_Backup_20250125')
BEGIN
    SELECT * INTO Donations_Backup_20250125 FROM Donations;
    PRINT '? Donations table backed up';
END
ELSE
    PRINT '? Donations backup already exists';
GO

-- Backup Accounts table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Accounts_Backup_20250125')
BEGIN
    SELECT * INTO Accounts_Backup_20250125 FROM Accounts;
    PRINT '? Accounts table backed up';
END
ELSE
    PRINT '? Accounts backup already exists';
GO

-- =============================================
-- SECTION 2: ADD NEW COLUMNS TO DONATIONS TABLE
-- =============================================
PRINT '';
PRINT 'SECTION 2: Updating Donations table structure...';
GO

-- Add TransactionId column (for SSLCommerz)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'TransactionId')
BEGIN
    ALTER TABLE Donations ADD TransactionId NVARCHAR(100) NULL;
    PRINT '? Added TransactionId column';
END
ELSE
    PRINT '? TransactionId column already exists';
GO

-- Add BankTransactionId column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'BankTransactionId')
BEGIN
    ALTER TABLE Donations ADD BankTransactionId NVARCHAR(100) NULL;
    PRINT '? Added BankTransactionId column';
END
ELSE
    PRINT '? BankTransactionId column already exists';
GO

-- Add CardType column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'CardType')
BEGIN
    ALTER TABLE Donations ADD CardType NVARCHAR(50) NULL;
    PRINT '? Added CardType column';
END
ELSE
    PRINT '? CardType column already exists';
GO

-- Add PaidAt column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'PaidAt')
BEGIN
    ALTER TABLE Donations ADD PaidAt DATETIME2 NULL;
    PRINT '? Added PaidAt column';
END
ELSE
    PRINT '? PaidAt column already exists';
GO

-- Add Currency column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'Currency')
BEGIN
    ALTER TABLE Donations ADD Currency NVARCHAR(10) DEFAULT 'BDT' NULL;
    PRINT '? Added Currency column';
END
ELSE
    PRINT '? Currency column already exists';
GO

-- =============================================
-- SECTION 3: CREATE INDEXES FOR PERFORMANCE
-- =============================================
PRINT '';
PRINT 'SECTION 3: Creating performance indexes...';
GO

-- Index on TransactionId for fast lookup
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Donations_TransactionId')
BEGIN
    CREATE INDEX IX_Donations_TransactionId ON Donations(TransactionId);
    PRINT '? Created index on TransactionId';
END
ELSE
    PRINT '? Index on TransactionId already exists';
GO

-- Index on PaymentMethod for reporting
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Donations_PaymentMethod')
BEGIN
    CREATE INDEX IX_Donations_PaymentMethod ON Donations(PaymentMethod);
    PRINT '? Created index on PaymentMethod';
END
ELSE
    PRINT '? Index on PaymentMethod already exists';
GO

-- Index on PaymentDate for reporting
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Donations_PaymentDate')
BEGIN
    CREATE INDEX IX_Donations_PaymentDate ON Donations(PaymentDate);
    PRINT '? Created index on PaymentDate';
END
ELSE
    PRINT '? Index on PaymentDate already exists';
GO

-- Index on Status for filtering
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Donations_Status')
BEGIN
    CREATE INDEX IX_Donations_Status ON Donations(Status);
    PRINT '? Created index on Status';
END
ELSE
    PRINT '? Index on Status already exists';
GO

-- =============================================
-- SECTION 4: UPDATE EXISTING DATA
-- =============================================
PRINT '';
PRINT 'SECTION 4: Updating existing records...';
GO

-- Set Currency to BDT for existing donations
UPDATE Donations 
SET Currency = 'BDT' 
WHERE Currency IS NULL;
PRINT '? Updated Currency for existing donations';
GO

-- Generate TransactionId for existing donations that don't have one
UPDATE Donations
SET TransactionId = 'WBS' + CONVERT(VARCHAR(8), CreatedAt, 112) + 
                    CONVERT(VARCHAR(6), CreatedAt, 114) + 
                    RIGHT('0000' + CAST(Id AS VARCHAR(4)), 4)
WHERE TransactionId IS NULL;
PRINT '? Generated TransactionId for existing donations';
GO

-- =============================================
-- SECTION 5: CREATE PAYMENT TRANSACTION LOG TABLE
-- =============================================
PRINT '';
PRINT 'SECTION 5: Creating payment transaction log...';
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PaymentTransactionLogs')
BEGIN
    CREATE TABLE PaymentTransactionLogs (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        DonationId INT NOT NULL,
        TransactionId NVARCHAR(100) NOT NULL,
        PaymentGateway NVARCHAR(50) NOT NULL, -- 'SSLCommerz', 'Manual', etc.
        RequestData NVARCHAR(MAX) NULL,
        ResponseData NVARCHAR(MAX) NULL,
        Status NVARCHAR(50) NOT NULL,
        ErrorMessage NVARCHAR(MAX) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        
        CONSTRAINT FK_PaymentLogs_Donations FOREIGN KEY (DonationId) 
            REFERENCES Donations(Id) ON DELETE CASCADE
    );
    
    CREATE INDEX IX_PaymentLogs_DonationId ON PaymentTransactionLogs(DonationId);
    CREATE INDEX IX_PaymentLogs_TransactionId ON PaymentTransactionLogs(TransactionId);
    CREATE INDEX IX_PaymentLogs_CreatedAt ON PaymentTransactionLogs(CreatedAt);
    
    PRINT '? Created PaymentTransactionLogs table';
END
ELSE
    PRINT '? PaymentTransactionLogs table already exists';
GO

-- =============================================
-- SECTION 6: CREATE VIEWS FOR REPORTING
-- =============================================
PRINT '';
PRINT 'SECTION 6: Creating reporting views...';
GO

-- Drop existing view if exists
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_DonationSummary')
    DROP VIEW vw_DonationSummary;
GO

CREATE VIEW vw_DonationSummary
AS
SELECT 
    d.Id,
    d.DonorName,
    d.Email,
    d.Phone,
    d.Amount,
    d.Currency,
    d.PaymentMethod,
    d.PaymentStatus,
    d.Status,
    d.TransactionId,
    d.BankTransactionId,
    d.CardType,
    d.CreatedAt,
    d.PaymentDate,
    d.PaidAt,
    dt.Name AS DonationType,
    dt.NameBn AS DonationTypeBn,
    a.Title AS AppealTitle,
    ac.AccountName,
    CASE 
        WHEN d.Status = 0 THEN 'Pending'
        WHEN d.Status = 1 THEN 'Completed'
        WHEN d.Status = 2 THEN 'Failed'
        WHEN d.Status = 3 THEN 'Cancelled'
        ELSE 'Unknown'
    END AS StatusText
FROM Donations d
LEFT JOIN DonationTypes dt ON d.DonationTypeId = dt.Id
LEFT JOIN Appeals a ON d.AppealId = a.Id
LEFT JOIN Accounts ac ON d.AccountId = ac.Id;
GO

PRINT '? Created vw_DonationSummary view';
GO

-- =============================================
-- SECTION 7: CREATE STORED PROCEDURES
-- =============================================
PRINT '';
PRINT 'SECTION 7: Creating stored procedures...';
GO

-- Drop existing procedure if exists
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetDonationByTransactionId')
    DROP PROCEDURE sp_GetDonationByTransactionId;
GO

CREATE PROCEDURE sp_GetDonationByTransactionId
    @TransactionId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        d.*,
        dt.Name AS DonationType,
        a.Title AS AppealTitle,
        ac.AccountName
    FROM Donations d
    LEFT JOIN DonationTypes dt ON d.DonationTypeId = dt.Id
    LEFT JOIN Appeals a ON d.AppealId = a.Id
    LEFT JOIN Accounts ac ON d.AccountId = ac.Id
    WHERE d.TransactionId = @TransactionId;
END
GO

PRINT '? Created sp_GetDonationByTransactionId procedure';
GO

-- Procedure to get payment statistics
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetPaymentStatistics')
    DROP PROCEDURE sp_GetPaymentStatistics;
GO

CREATE PROCEDURE sp_GetPaymentStatistics
    @StartDate DATETIME2 = NULL,
    @EndDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Default to last 30 days if not specified
    IF @StartDate IS NULL
        SET @StartDate = DATEADD(DAY, -30, GETUTCDATE());
    
    IF @EndDate IS NULL
        SET @EndDate = GETUTCDATE();
    
    SELECT 
        PaymentMethod,
        COUNT(*) AS TotalTransactions,
        SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END) AS SuccessfulTransactions,
        SUM(CASE WHEN Status = 2 THEN 1 ELSE 0 END) AS FailedTransactions,
        SUM(CASE WHEN Status = 0 THEN 1 ELSE 0 END) AS PendingTransactions,
        SUM(Amount) AS TotalAmount,
        SUM(CASE WHEN Status = 1 THEN Amount ELSE 0 END) AS SuccessfulAmount,
        AVG(Amount) AS AverageAmount
    FROM Donations
    WHERE CreatedAt BETWEEN @StartDate AND @EndDate
    GROUP BY PaymentMethod;
END
GO

PRINT '? Created sp_GetPaymentStatistics procedure';
GO

-- =============================================
-- SECTION 8: DATA VALIDATION
-- =============================================
PRINT '';
PRINT 'SECTION 8: Validating data integrity...';
GO

-- Check for donations without TransactionId
DECLARE @MissingTransactionId INT;
SELECT @MissingTransactionId = COUNT(*) FROM Donations WHERE TransactionId IS NULL;

IF @MissingTransactionId > 0
    PRINT '? WARNING: ' + CAST(@MissingTransactionId AS VARCHAR) + ' donations without TransactionId';
ELSE
    PRINT '? All donations have TransactionId';
GO

-- Check for completed donations without PaymentDate
DECLARE @MissingPaymentDate INT;
SELECT @MissingPaymentDate = COUNT(*) 
FROM Donations 
WHERE Status = 1 AND PaymentDate IS NULL;

IF @MissingPaymentDate > 0
BEGIN
    PRINT '? Fixing ' + CAST(@MissingPaymentDate AS VARCHAR) + ' completed donations without PaymentDate';
    UPDATE Donations 
    SET PaymentDate = CreatedAt, 
        PaidAt = CreatedAt
    WHERE Status = 1 AND PaymentDate IS NULL;
END
ELSE
    PRINT '? All completed donations have PaymentDate';
GO

-- =============================================
-- SECTION 9: GRANT PERMISSIONS (Optional)
-- =============================================
PRINT '';
PRINT 'SECTION 9: Setting permissions...';
GO

-- Grant execute permissions on stored procedures
-- GRANT EXECUTE ON sp_GetDonationByTransactionId TO [YourAppUser];
-- GRANT EXECUTE ON sp_GetPaymentStatistics TO [YourAppUser];
PRINT '? Permissions setup skipped - configure manually if needed';
GO

-- =============================================
-- SECTION 10: FINAL VERIFICATION
-- =============================================
PRINT '';
PRINT 'SECTION 10: Final verification...';
GO

-- Count records
DECLARE @TotalDonations INT;
DECLARE @CompletedDonations INT;
DECLARE @TotalAmount DECIMAL(18,2);

SELECT 
    @TotalDonations = COUNT(*),
    @CompletedDonations = SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END),
    @TotalAmount = SUM(CASE WHEN Status = 1 THEN Amount ELSE 0 END)
FROM Donations;

PRINT '';
PRINT '========================================';
PRINT 'Migration Summary:';
PRINT '========================================';
PRINT 'Total Donations: ' + CAST(@TotalDonations AS VARCHAR);
PRINT 'Completed Donations: ' + CAST(@CompletedDonations AS VARCHAR);
PRINT 'Total Amount Raised: BDT ' + CAST(@TotalAmount AS VARCHAR);
PRINT '';
PRINT '? Database migration completed successfully!';
PRINT 'Timestamp: ' + CONVERT(VARCHAR, GETDATE(), 121);
PRINT '========================================';
GO

-- =============================================
-- VERIFICATION QUERIES
-- =============================================
PRINT '';
PRINT 'Running verification queries...';
GO

-- Show sample of updated donations
SELECT TOP 5 
    Id, DonorName, Amount, PaymentMethod, 
    TransactionId, Status, CreatedAt, PaymentDate
FROM Donations
ORDER BY CreatedAt DESC;
GO

-- Show payment method distribution
SELECT 
    PaymentMethod,
    COUNT(*) AS Count,
    SUM(Amount) AS TotalAmount
FROM Donations
GROUP BY PaymentMethod
ORDER BY COUNT(*) DESC;
GO

PRINT '';
PRINT '? All migration tasks completed!';
PRINT '';
GO
