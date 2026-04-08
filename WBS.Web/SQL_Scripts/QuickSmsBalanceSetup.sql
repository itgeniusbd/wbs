-- ============================================
-- SMS Balance Quick Setup - ???? ???? ????
-- ============================================

-- Step 1: Check if table exists and has data
SELECT 'Current Balance:' AS Info, * FROM SmsBalances;

-- Step 2: Set initial balance (????? SMS balance ????? ???)
-- ??????: 500 ?? SMS ?? ????
UPDATE SmsBalances
SET AvailableBalance = 500,
    LastUpdated = GETDATE(),
    UpdatedBy = 'Admin',
    Notes = 'Initial setup - 500 SMS credits purchased'
WHERE Id = 1;

-- Step 3: Verify the update
SELECT 
    'Updated Balance:' AS Info,
    AvailableBalance AS Balance,
    LastUpdated,
    UpdatedBy,
    Notes
FROM SmsBalances
WHERE Id = 1;

-- ============================================
-- SMS Testing Queries
-- ============================================

-- Check recent SMS attempts
SELECT TOP 10
    PhoneNumber,
    DonorName,
    CASE Status
        WHEN 1 THEN '? Success'
        WHEN 2 THEN '? Failed'
        WHEN 3 THEN '?? Insufficient Balance'
        WHEN 4 THEN '? Invalid Number'
        ELSE '? Unknown'
    END AS Status,
    BalanceBefore,
    BalanceAfter,
    SentAt
FROM SmsLogs
ORDER BY SentAt DESC;

-- ============================================
-- Balance ???? ??? ???? Alert
-- ============================================
DECLARE @CurrentBalance INT;
SELECT @CurrentBalance = AvailableBalance FROM SmsBalances WHERE Id = 1;

IF @CurrentBalance < 50
BEGIN
    PRINT '?? WARNING: SMS Balance is low! Current balance: ' + CAST(@CurrentBalance AS VARCHAR(10));
    PRINT 'Please recharge soon!';
END
ELSE
BEGIN
    PRINT '? SMS Balance is sufficient. Current balance: ' + CAST(@CurrentBalance AS VARCHAR(10));
END

-- ============================================
-- Balance recharge ???? ????
-- ============================================
-- ????? script use ??? balance add ???? ??????
-- ??????: ??? 1000 SMS add ???? ????

/*
DECLARE @CurrentBalance INT;
DECLARE @AddAmount INT = 1000; -- ???? SMS add ?????

SELECT @CurrentBalance = AvailableBalance FROM SmsBalances WHERE Id = 1;

UPDATE SmsBalances
SET AvailableBalance = AvailableBalance + @AddAmount,
    LastUpdated = GETDATE(),
    UpdatedBy = 'Admin',
    Notes = 'Recharged with ' + CAST(@AddAmount AS VARCHAR(10)) + ' SMS credits'
WHERE Id = 1;

PRINT '? Balance updated successfully!';
PRINT 'Previous Balance: ' + CAST(@CurrentBalance AS VARCHAR(10));
PRINT 'Added: ' + CAST(@AddAmount AS VARCHAR(10));
PRINT 'New Balance: ' + CAST(@CurrentBalance + @AddAmount AS VARCHAR(10));
*/
