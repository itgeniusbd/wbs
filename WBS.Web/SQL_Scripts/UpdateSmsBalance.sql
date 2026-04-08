-- ============================================
-- SMS Balance Management SQL Scripts
-- ============================================

-- 1. Check current SMS balance
SELECT TOP 1 
    Id,
    AvailableBalance,
    LastUpdated,
    UpdatedBy,
    Notes
FROM SmsBalances
ORDER BY Id DESC;

-- ============================================
-- 2. Update SMS balance (Change the number 1000 to your desired balance)
-- ============================================
UPDATE SmsBalances
SET AvailableBalance = 1000,  -- ????? SMS balance ?????? ????? ???
    LastUpdated = GETDATE(),
    UpdatedBy = 'Admin',
    Notes = 'Balance updated manually'
WHERE Id = (SELECT TOP 1 Id FROM SmsBalances ORDER BY Id DESC);

-- ============================================
-- 3. View recent SMS logs (Last 50 SMS)
-- ============================================
SELECT TOP 50
    Id,
    PhoneNumber,
    DonorName,
    Amount,
    Status,
    CASE Status
        WHEN 1 THEN 'Success'
        WHEN 2 THEN 'Failed'
        WHEN 3 THEN 'Insufficient Balance'
        WHEN 4 THEN 'Invalid Number'
        ELSE 'Unknown'
    END AS StatusText,
    ErrorMessage,
    BalanceBefore,
    BalanceAfter,
    SentAt
FROM SmsLogs
ORDER BY SentAt DESC;

-- ============================================
-- 4. View today's SMS statistics
-- ============================================
SELECT 
    COUNT(*) AS TotalSMS,
    SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END) AS SuccessCount,
    SUM(CASE WHEN Status = 2 THEN 1 ELSE 0 END) AS FailedCount,
    SUM(CASE WHEN Status = 3 THEN 1 ELSE 0 END) AS InsufficientBalanceCount,
    SUM(CASE WHEN Status = 4 THEN 1 ELSE 0 END) AS InvalidNumberCount
FROM SmsLogs
WHERE CAST(SentAt AS DATE) = CAST(GETDATE() AS DATE);

-- ============================================
-- 5. View SMS logs by donor name
-- ============================================
-- Replace 'DonorName' with actual donor name
SELECT 
    PhoneNumber,
    DonorName,
    Amount,
    Message,
    Status,
    SentAt
FROM SmsLogs
WHERE DonorName LIKE '%DonorName%'
ORDER BY SentAt DESC;

-- ============================================
-- 6. View failed SMS only
-- ============================================
SELECT 
    PhoneNumber,
    DonorName,
    Message,
    ErrorMessage,
    SentAt
FROM SmsLogs
WHERE Status IN (2, 3, 4)  -- Failed, Insufficient Balance, Invalid Number
ORDER BY SentAt DESC;

-- ============================================
-- 7. Monthly SMS usage report
-- ============================================
SELECT 
    YEAR(SentAt) AS Year,
    MONTH(SentAt) AS Month,
    COUNT(*) AS TotalSMS,
    SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END) AS SuccessCount,
    SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END) AS SMSCost
FROM SmsLogs
GROUP BY YEAR(SentAt), MONTH(SentAt)
ORDER BY Year DESC, Month DESC;

-- ============================================
-- 8. Delete old SMS logs (older than 6 months)
-- ============================================
-- Uncomment below to delete old logs
-- DELETE FROM SmsLogs
-- WHERE SentAt < DATEADD(MONTH, -6, GETDATE());
