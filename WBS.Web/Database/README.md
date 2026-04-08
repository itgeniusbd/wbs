# ??? Database Migration Scripts - WBS Bangladesh

## ?? Overview

?? folder ? **SSLCommerz Payment Gateway Integration** ?? ???? ???????? database migration scripts ???????

### Files in This Directory:

1. **Migration_20250125_SSLCommerz.sql** - Main migration script
2. **Rollback_20250125_SSLCommerz.sql** - Rollback script (if needed)
3. **Verify_Migration.sql** - Verification script
4. **DEPLOYMENT_GUIDE.md** - Complete deployment guide

---

## ?? What Does the Migration Do?

### ? New Columns Added to `Donations` Table:

| Column Name | Type | Description |
|------------|------|-------------|
| `TransactionId` | NVARCHAR(100) | Unique transaction ID from SSLCommerz |
| `BankTransactionId` | NVARCHAR(100) | Bank transaction reference |
| `CardType` | NVARCHAR(50) | Card type used (VISA, MasterCard, etc.) |
| `PaidAt` | DATETIME2 | Actual payment timestamp |
| `Currency` | NVARCHAR(10) | Currency code (default: BDT) |

### ? New Tables Created:

| Table Name | Purpose |
|-----------|---------|
| `PaymentTransactionLogs` | Detailed logs of all payment transactions |
| `Donations_Backup_20250125` | Automatic backup before migration |
| `Accounts_Backup_20250125` | Automatic backup before migration |

### ? Performance Indexes:

- `IX_Donations_TransactionId` - Fast lookup by transaction ID
- `IX_Donations_PaymentMethod` - Payment method filtering
- `IX_Donations_PaymentDate` - Date-based reporting
- `IX_Donations_Status` - Status filtering

### ? Database Objects:

- **Views**: `vw_DonationSummary` - Comprehensive donation report view
- **Stored Procedures**: 
  - `sp_GetDonationByTransactionId` - Get donation details
  - `sp_GetPaymentStatistics` - Payment statistics report

---

## ?? Quick Start Guide

### For Development/Testing:

```sql
-- 1. Backup your database first (manually or using script)
BACKUP DATABASE WBS_NGO TO DISK = 'C:\Backups\WBS_NGO_Backup.bak';

-- 2. Run migration script
-- Open Migration_20250125_SSLCommerz.sql in SSMS and execute

-- 3. Verify migration
-- Open Verify_Migration.sql and execute

-- 4. Check results
-- All checks should show ? (checkmark)
```

### For Production:

**Follow the complete guide in:** `DEPLOYMENT_GUIDE.md`

---

## ?? Pre-Migration Checklist

Before running migration:

- [ ] Database backup created
- [ ] Migration script reviewed
- [ ] Tested in development environment
- [ ] Tested in staging environment
- [ ] Rollback plan prepared
- [ ] Maintenance window scheduled
- [ ] Stakeholders notified

---

## ?? How to Run Migration

### Method 1: SQL Server Management Studio (SSMS)

1. **Open SSMS**
2. **Connect** to your server
3. **File ? Open ? File**
4. Select `Migration_20250125_SSLCommerz.sql`
5. **Review** the script
6. Click **Execute** (F5)
7. **Review** output messages

### Method 2: Command Line (sqlcmd)

```bash
# Navigate to scripts directory
cd F:\WBS\WBS.Web\Database\

# Run migration
sqlcmd -S YOUR_SERVER_NAME -d WBS_NGO -E -i Migration_20250125_SSLCommerz.sql -o migration_log.txt

# Check log
type migration_log.txt
```

### Method 3: Azure Data Studio

1. Open Azure Data Studio
2. Connect to server
3. Open `Migration_20250125_SSLCommerz.sql`
4. Click **Run**
5. Review results

---

## ? Verification

### After Migration, Run:

```sql
-- Execute verification script
-- This will check:
-- ? All columns exist
-- ? All indexes created
-- ? All procedures created
-- ? All views created
-- ? Data integrity maintained
```

### Expected Output:

```
========================================
WBS Database Verification Script
========================================

1. Database Information: ?
2. Verifying table structure: ?
3. Verifying indexes: ?
4. Verifying stored procedures: ?
5. Verifying views: ?
6. Verifying PaymentTransactionLogs table: ?
7. Data integrity checks: ?
8. Verifying backup tables: ?

? ALL CHECKS PASSED!
Database is ready for production use.
========================================
```

---

## ?? Rollback Procedure

If something goes wrong:

### Option 1: Use Rollback Script

```sql
-- 1. Open Rollback_20250125_SSLCommerz.sql
-- 2. Change @Confirm from 'NO' to 'YES'
-- 3. Execute the script
```

?? **Warning**: This will remove all changes made by migration!

### Option 2: Restore from Backup

```sql
USE master;
GO

-- Restore database
RESTORE DATABASE WBS_NGO 
FROM DISK = 'C:\Backups\WBS_NGO_Backup.bak'
WITH REPLACE, RECOVERY;
GO
```

---

## ?? What Gets Backed Up?

### Automatic Backups (Created by Migration Script):

1. **Donations_Backup_20250125** - Complete snapshot of Donations table
2. **Accounts_Backup_20250125** - Complete snapshot of Accounts table

### These backups are:
- ? Created automatically before any changes
- ? Include ALL existing data
- ? Can be used for rollback
- ? Can be deleted after verification (7+ days)

### To Remove Backups (After Successful Deployment):

```sql
-- After 7 days, if everything is stable
DROP TABLE IF EXISTS Donations_Backup_20250125;
DROP TABLE IF EXISTS Accounts_Backup_20250125;
```

---

## ?? Migration Steps in Detail

### The migration script performs these steps:

1. **Backup** - Creates backup tables
2. **Add Columns** - Adds new columns to Donations table
3. **Create Indexes** - Performance optimization
4. **Update Data** - Sets default values for existing records
5. **Create Tables** - PaymentTransactionLogs table
6. **Create Views** - Reporting views
7. **Create Procedures** - Stored procedures
8. **Validate** - Data integrity checks
9. **Report** - Summary statistics

### Total Time: ~30 seconds to 2 minutes
(Depends on number of existing donations)

---

## ?? Database Size Impact

### Before Migration:
- Donations table: ~X MB

### After Migration:
- Donations table: ~X + 2 MB
- PaymentTransactionLogs: ~0.1 MB
- Backup tables: ~X * 2 MB (temporary)
- Indexes: ~1 MB

### Total Additional Space: ~X * 2 + 3 MB

?? Ensure sufficient disk space (at least 3x current database size)

---

## ?? Testing Queries

### After migration, test with these queries:

```sql
-- 1. Check new columns
SELECT TOP 5 
    Id, DonorName, Amount, 
    TransactionId, CardType, Currency
FROM Donations;

-- 2. Test view
SELECT * FROM vw_DonationSummary 
ORDER BY CreatedAt DESC;

-- 3. Test stored procedure
EXEC sp_GetPaymentStatistics 
    @StartDate = '2025-01-01', 
    @EndDate = GETDATE();

-- 4. Check indexes are being used
SELECT 
    OBJECT_NAME(s.object_id) AS TableName,
    i.name AS IndexName,
    s.user_seeks + s.user_scans + s.user_lookups AS TotalReads
FROM sys.dm_db_index_usage_stats s
INNER JOIN sys.indexes i ON s.object_id = i.object_id 
    AND s.index_id = i.index_id
WHERE OBJECT_NAME(s.object_id) = 'Donations'
ORDER BY TotalReads DESC;
```

---

## ?? Support

### If You Encounter Issues:

1. **Check Output Log** - Review migration output for errors
2. **Run Verification** - Execute `Verify_Migration.sql`
3. **Review Backup** - Ensure backups exist
4. **Contact DBA** - If critical errors occur

### Common Issues:

| Issue | Solution |
|-------|----------|
| Column already exists | Migration partially ran - check verification script |
| Permission denied | Ensure user has db_owner or db_ddladmin role |
| Timeout error | Increase query timeout or split migration |
| Foreign key error | Check if referenced tables exist |

---

## ?? Migration History

| Date | Version | Description | Status |
|------|---------|-------------|--------|
| 2025-01-25 | 1.0.0 | SSLCommerz Integration | ? Ready |

---

## ?? Success Criteria

Migration is successful when:

? All verification checks pass  
? No errors in migration output  
? Backup tables created  
? Sample queries return data  
? Application connects successfully  
? Test transaction completes  

---

## ?? Emergency Rollback

If critical issues occur in production:

```sql
-- EMERGENCY ROLLBACK (USE WITH CAUTION!)

-- 1. Stop application
-- 2. Run this:

USE master;
GO

ALTER DATABASE WBS_NGO SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

RESTORE DATABASE WBS_NGO 
FROM DISK = 'YOUR_BACKUP_PATH.bak'
WITH REPLACE, RECOVERY;
GO

ALTER DATABASE WBS_NGO SET MULTI_USER;
GO

-- 3. Restart application with old version
```

---

## ?? Additional Resources

- **Main Deployment Guide**: `DEPLOYMENT_GUIDE.md`
- **SSLCommerz Setup**: `../SSLCOMMERZ_LIVE_FINAL.md`
- **Localhost Issues**: `../LOCALHOST_LIVE_MODE_ISSUE.md`

---

## ? Benefits After Migration

### For Users:
- ? Online payment option available
- ? Multiple payment methods
- ? Secure transactions
- ? Instant confirmation

### For Admins:
- ? Automatic payment tracking
- ? Better reporting
- ? Payment analytics
- ? Transaction logs

### For System:
- ? Faster queries (indexes)
- ? Better data integrity
- ? Audit trail
- ? Scalability

---

**Migration Script Version**: 1.0.0  
**Created**: January 25, 2025  
**Tested On**: SQL Server 2019+  
**Compatible With**: .NET 8 Application  

**Status**: ? Production Ready
