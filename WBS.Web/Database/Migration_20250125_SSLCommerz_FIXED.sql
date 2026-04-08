-- =============================================
-- WBS Bangladesh - Production Database Migration (FIXED)
-- SSLCommerz Payment Gateway Integration
-- Date: January 25, 2025
-- Version: 1.0.1 (Fixed for existing database)
-- =============================================

USE WBS_NGO;
GO

PRINT '========================================';
PRINT 'Starting WBS Production Database Migration (FIXED)';
PRINT 'Timestamp: ' + CONVERT(VARCHAR, GETDATE(), 121);
PRINT '========================================';
GO

-- =============================================
-- SECTION 1: BACKUP EXISTING DATA
-- =============================================
PRINT '';
PRINT 'SECTION 1: Creating backup tables...';
GO

-- Backup Donations table (with new name to avoid conflicts)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Donations_Backup_20250125_v2')
BEGIN
    SELECT * INTO Donations_Backup_20250125_v2 FROM Donations;
    PRINT '? Donations table backed up to Donations_Backup_20250125_v2';
END
ELSE
    PRINT '? Donations backup already exists';
GO

-- =============================================
-- SECTION 2: ADD NEW COLUMNS (IF NOT EXISTS)
-- =============================================
PRINT '';
PRINT 'SECTION 2: Updating Donations table structure...';
GO

-- Add TransactionId column (if not exists)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'TransactionId')
BEGIN
    ALTER TABLE Donations ADD TransactionId NVARCHAR(100) NULL;
    PRINT '? Added TransactionId column';
END
ELSE
    PRINT '? TransactionId column already exists - skipping';
GO

-- Add BankTransactionId column (if not exists)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'BankTransactionId')
BEGIN
    ALTER TABLE Donations ADD BankTransactionId NVARCHAR(100) NULL;
    PRINT '? Added BankTransactionId column';
END
ELSE
    PRINT '? BankTransactionId column already exists - skipping';
GO

-- Add CardType column (if not exists)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'CardType')
BEGIN
    ALTER TABLE Donations ADD CardType NVARCHAR(50) NULL;
    PRINT '? Added CardType column';
END
ELSE
    PRINT '? CardType column already exists - skipping';
GO

-- Add PaidAt column (if not exists)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'PaidAt')
BEGIN
    ALTER TABLE Donations ADD PaidAt DATETIME2 NULL;
    PRINT '? Added PaidAt column';
END
ELSE
    PRINT '? PaidAt column already exists - skipping';
GO

-- Add Currency column (if not exists)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'Currency')
BEGIN
    ALTER TABLE Donations ADD Currency NVARCHAR(10) NULL DEFAULT 'BDT';
    PRINT '? Added Currency column';
END
ELSE
    PRINT '? Currency column already exists - skipping';
GO

-- =============================================
-- SECTION 3: CREATE INDEXES (IF NOT EXISTS)
-- =============================================
PRINT '';
PRINT 'SECTION 3: Creating performance indexes...';
GO

-- Index on TransactionId
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Donations_TransactionId' AND object_id = OBJECT_ID('Donations'))
BEGIN
    CREATE INDEX IX_Donations_TransactionId ON Donations(TransactionId);
    PRINT '? Created index on TransactionId';
END
ELSE
    PRINT '? Index on TransactionId already exists';
GO

-- Index on PaymentMethod
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Donations_PaymentMethod' AND object_id = OBJECT_ID('Donations'))
BEGIN
    CREATE INDEX IX_Donations_PaymentMethod ON Donations(PaymentMethod);
    PRINT '? Created index on PaymentMethod';
END
ELSE
    PRINT '? Index on PaymentMethod already exists';
GO

-- Index on CreatedAt (instead of PaymentDate which might not exist)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Donations_CreatedAt' AND object_id = OBJECT_ID('Donations'))
BEGIN
    CREATE INDEX IX_Donations_CreatedAt ON Donations(CreatedAt);
    PRINT '? Created index on CreatedAt';
END
ELSE
    PRINT '? Index on CreatedAt already exists';
GO

-- Index on Status
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Donations_Status' AND object_id = OBJECT_ID('Donations'))
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

-- Set Currency to BDT for existing donations (where NULL)
UPDATE Donations 
SET Currency = 'BDT' 
WHERE Currency IS NULL;
PRINT '? Updated Currency for existing donations';
GO

-- Generate TransactionId for existing donations that don't have one
UPDATE Donations
SET TransactionId = 'WBS' + 
    CONVERT(VARCHAR(8), CreatedAt, 112) + 
    REPLACE(CONVERT(VARCHAR(8), CreatedAt, 108), ':', '') + 
    RIGHT('0000' + CAST(Id AS VARCHAR(4)), 4)
WHERE TransactionId IS NULL;
PRINT '? Generated TransactionId for existing donations';
GO

-- =============================================
-- SECTION 5: CREATE PAYMENT TRANSACTION LOG
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
        PaymentGateway NVARCHAR(50) NOT NULL,
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
    
    PRINT '? Created PaymentTransactionLogs table with indexes';
END
ELSE
    PRINT '? PaymentTransactionLogs table already exists';
GO

-- =============================================
-- SECTION 6: CREATE/UPDATE VIEWS
-- =============================================
PRINT '';
PRINT 'SECTION 6: Creating/updating views...';
GO

-- Drop and recreate view to ensure compatibility
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
    ISNULL(d.Currency, 'BDT') AS Currency,
    d.PaymentMethod,
    d.PaymentStatus,
    d.Status,
    d.TransactionId,
    d.BankTransactionId,
    d.CardType,
    d.CreatedAt,
    d.PaidAt,
    dt.Name AS DonationType,
    dt.NameBn AS DonationTypeBn,
    CASE 
        WHEN d.Status = 0 THEN 'Pending'
        WHEN d.Status = 1 THEN 'Completed'
        WHEN d.Status = 2 THEN 'Failed'
        WHEN d.Status = 3 THEN 'Cancelled'
        ELSE 'Unknown'
    END AS StatusText
FROM Donations d
LEFT JOIN DonationTypes dt ON d.DonationTypeId = dt.Id;
GO

PRINT '? Created/Updated vw_DonationSummary view';
GO

-- =============================================
-- SECTION 7: CREATE STORED PROCEDURES
-- =============================================
PRINT '';
PRINT 'SECTION 7: Creating stored procedures...';
GO

-- Drop and recreate procedures
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
        dt.Name AS DonationType
    FROM Donations d
    LEFT JOIN DonationTypes dt ON d.DonationTypeId = dt.Id
    WHERE d.TransactionId = @TransactionId;
END
GO

PRINT '? Created sp_GetDonationByTransactionId procedure';
GO

-- Payment statistics procedure
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetPaymentStatistics')
    DROP PROCEDURE sp_GetPaymentStatistics;
GO

CREATE PROCEDURE sp_GetPaymentStatistics
    @StartDate DATETIME2 = NULL,
    @EndDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
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
    GROUP BY PaymentMethod
    ORDER BY TotalTransactions DESC;
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

DECLARE @MissingTransactionId INT;
SELECT @MissingTransactionId = COUNT(*) FROM Donations WHERE TransactionId IS NULL;

IF @MissingTransactionId > 0
    PRINT '? WARNING: ' + CAST(@MissingTransactionId AS VARCHAR) + ' donations without TransactionId';
ELSE
    PRINT '? All donations have TransactionId';
GO

-- Fix missing TransactionId (if any)
IF EXISTS (SELECT 1 FROM Donations WHERE TransactionId IS NULL)
BEGIN
    UPDATE Donations
    SET TransactionId = 'WBS' + 
        CONVERT(VARCHAR(8), GETDATE(), 112) + 
        REPLACE(CONVERT(VARCHAR(8), GETDATE(), 108), ':', '') + 
        RIGHT('0000' + CAST(Id AS VARCHAR(4)), 4)
    WHERE TransactionId IS NULL;
    PRINT '? Fixed missing TransactionId';
END
GO

-- =============================================
-- SECTION 9: FINAL VERIFICATION
-- =============================================
PRINT '';
PRINT 'SECTION 9: Final verification...';
GO

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
PRINT 'Total Amount Raised: BDT ' + CAST(ISNULL(@TotalAmount, 0) AS VARCHAR);
PRINT '';

-- Check all new columns exist
DECLARE @AllColumnsExist BIT = 1;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'TransactionId')
    SET @AllColumnsExist = 0;
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'BankTransactionId')
    SET @AllColumnsExist = 0;
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'CardType')
    SET @AllColumnsExist = 0;
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'PaidAt')
    SET @AllColumnsExist = 0;
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Donations') AND name = 'Currency')
    SET @AllColumnsExist = 0;

IF @AllColumnsExist = 1
BEGIN
    PRINT '? All required columns exist';
    PRINT '? Database migration completed successfully!';
END
ELSE
BEGIN
    PRINT '? Some columns are missing!';
    PRINT '? Please review the output above';
END

PRINT '';
PRINT 'Timestamp: ' + CONVERT(VARCHAR, GETDATE(), 121);
PRINT '========================================';
GO

-- Show sample data
PRINT '';
PRINT 'Sample of updated donations:';
SELECT TOP 5 
    Id, DonorName, Amount, PaymentMethod, 
    TransactionId, Currency, Status, CreatedAt
FROM Donations
ORDER BY CreatedAt DESC;
GO

PRINT '';
PRINT '? Migration completed!';
PRINT '';
GO
