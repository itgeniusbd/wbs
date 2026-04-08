-- Fix Bengali Text Encoding Issue
-- Run this script in SQL Server Management Studio or using Package Manager Console

-- Step 1: Check current collation
SELECT 
    SERVERPROPERTY('Collation') AS ServerCollation,
    DATABASEPROPERTYEX('WBS_NGO', 'Collation') AS DatabaseCollation;

-- Step 2: Update DonationTypes with proper Bengali text
UPDATE DonationTypes SET NameBn = N'???????' WHERE Id = 1 AND Name = 'Lillah';
UPDATE DonationTypes SET NameBn = N'?????' WHERE Id = 2 AND Name = 'Zakat';
UPDATE DonationTypes SET NameBn = N'??????? ????????' WHERE Id = 3 AND Name = 'Sadaqah Jariyah';
UPDATE DonationTypes SET NameBn = N'???????? ?????' WHERE Id = 4 AND Name = 'Winter Appeal';
UPDATE DonationTypes SET NameBn = N'????? ?????' WHERE Id = 5 AND Name = 'Emergency Appeal';

-- Step 3: Verify the update
SELECT Id, Name, NameBn FROM DonationTypes;

-- Step 4: If you have existing Sliders, update them
-- Example: UPDATE Sliders SET TitleBn = N'???? ?????' WHERE Title = 'Water Appeal';
-- Example: UPDATE Sliders SET SubtitleBn = N'????? ????? ?????? ?????' WHERE Subtitle = 'The Gift of Water is the Gift of Life';
-- Example: UPDATE Sliders SET ButtonTextBn = N'???? ??? ????' WHERE ButtonText = 'Donate Now';

PRINT 'Bengali text has been updated successfully!';
