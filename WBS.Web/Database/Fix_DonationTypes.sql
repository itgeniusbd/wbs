-- =============================================
-- Quick Fix: Check and Add Donation Types
-- Run this on production server if DonationTypes are missing
-- =============================================

USE WBS_NGO;
GO

PRINT '========================================';
PRINT 'Checking Donation Types';
PRINT '========================================';
GO

-- Check if DonationTypes exist
DECLARE @Count INT;
SELECT @Count = COUNT(*) FROM DonationTypes;

PRINT 'Current DonationTypes count: ' + CAST(@Count AS VARCHAR);
GO

-- If no donation types, add default ones
IF NOT EXISTS (SELECT 1 FROM DonationTypes WHERE IsActive = 1)
BEGIN
    PRINT 'No active Donation Types found. Adding default types...';
    
    -- Insert default donation types
    SET IDENTITY_INSERT DonationTypes ON;
    
    INSERT INTO DonationTypes (Id, Name, NameBn, Description, DescriptionBn, Icon, IsActive, DisplayOrder)
    VALUES
    (1, 'General Donation', '?????? ???', 'Support our general activities', '?????? ?????? ????????? ?????? ????', 'fas fa-hand-holding-heart', 1, 1),
    (2, 'Education', '??????', 'Support education programs', '?????? ???????? ?????? ????', 'fas fa-graduation-cap', 1, 2),
    (3, 'Healthcare', '?????????????', 'Support healthcare initiatives', '????????????? ?????? ?????? ????', 'fas fa-heartbeat', 1, 3),
    (4, 'Emergency Relief', '????? ?????', 'Emergency and disaster relief', '????? ? ??????? ?????', 'fas fa-hands-helping', 1, 4),
    (5, 'Zakat', '?????', 'Zakat donations', '????? ??????', 'fas fa-mosque', 1, 5),
    (6, 'Sadaqah', '???????', 'Sadaqah donations', '??????? ??????', 'fas fa-donate', 1, 6),
    (7, 'Orphan Support', '???? ???????', 'Support orphaned children', '???? ??????? ???????', 'fas fa-child', 1, 7);
    
    SET IDENTITY_INSERT DonationTypes OFF;
    
    PRINT '? Default Donation Types added successfully!';
END
ELSE
BEGIN
    PRINT '? Donation Types already exist.';
END
GO

-- Show current donation types
PRINT '';
PRINT 'Current Donation Types:';
SELECT 
    Id, 
    Name, 
    NameBn, 
    IsActive, 
    DisplayOrder
FROM DonationTypes
ORDER BY DisplayOrder;
GO

-- Check Accounts
PRINT '';
PRINT '========================================';
PRINT 'Checking Accounts';
PRINT '========================================';
GO

DECLARE @AccountCount INT;
SELECT @AccountCount = COUNT(*) FROM Accounts WHERE IsActive = 1;

PRINT 'Active Accounts count: ' + CAST(@AccountCount AS VARCHAR);

IF @AccountCount = 0
BEGIN
    PRINT '?? WARNING: No active accounts found!';
    PRINT 'Please add at least one account from admin panel.';
END
ELSE
BEGIN
    PRINT '? Active accounts found.';
    
    -- Show accounts
    SELECT 
        Id,
        AccountName,
        AccountType,
        AccountBalance,
        Default_Status,
        IsActive
    FROM Accounts
    WHERE IsActive = 1;
END
GO

PRINT '';
PRINT '========================================';
PRINT 'Verification Complete';
PRINT '========================================';
GO
