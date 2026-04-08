-- Complete Bangladesh Upazilas Data - FIXED VERSION
-- This script handles duplicate districts and cleans existing data
-- IMPORTANT: Run this AFTER running the Districts insert/update scripts

PRINT 'Starting Upazila Insert Script...';
PRINT '';

-- =======================
-- PRE-CHECK: Verify Districts Table
-- =======================
PRINT 'Checking Districts table...';
DECLARE @DistrictCount INT = (SELECT COUNT(*) FROM Districts);
PRINT 'Found ' + CAST(@DistrictCount AS VARCHAR(10)) + ' districts in database';

IF @DistrictCount < 64
BEGIN
    PRINT 'ERROR: Expected 64 districts, found ' + CAST(@DistrictCount AS VARCHAR(10));
    PRINT 'Please run BangladeshDistricts.sql first!';
    RETURN;
END

-- Check for duplicate districts
IF EXISTS (SELECT Name, COUNT(*) FROM Districts GROUP BY Name HAVING COUNT(*) > 1)
BEGIN
    PRINT 'WARNING: Duplicate districts found! Showing duplicates:';
    SELECT Name, COUNT(*) as [Count] FROM Districts GROUP BY Name HAVING COUNT(*) > 1;
    PRINT '';
    PRINT 'Please clean up duplicates first or this script will use the first match.';
    PRINT '';
END

-- =======================
-- CLEANUP: Remove existing upazilas (optional)
-- =======================
DECLARE @ExistingUpazilas INT = (SELECT COUNT(*) FROM Upazilas);
IF @ExistingUpazilas > 0
BEGIN
    PRINT 'Found ' + CAST(@ExistingUpazilas AS VARCHAR(10)) + ' existing upazilas.';
    PRINT 'Clearing existing upazila data...';
    DELETE FROM Upazilas;
    PRINT 'Existing upazilas cleared.';
END
ELSE
BEGIN
    PRINT 'No existing upazilas found. Starting fresh insert.';
END
PRINT '';

-- =======================
-- DHAKA DIVISION
-- =======================
PRINT 'Inserting Dhaka Division upazilas...';

-- Dhaka District (23 upazilas)
DECLARE @DhakaId INT = (SELECT TOP 1 Id FROM Districts WHERE Name = 'Dhaka' ORDER BY Id);
IF @DhakaId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Dhamrai', N'ধামরাই', @DhakaId, 0, 1, GETDATE()),
    (N'Dohar', N'দোহার', @DhakaId, 0, 2, GETDATE()),
    (N'Keraniganj', N'কেরানীগঞ্জ', @DhakaId, 0, 3, GETDATE()),
    (N'Nawabganj', N'নবাবগঞ্জ', @DhakaId, 0, 4, GETDATE()),
    (N'Savar', N'সাভার', @DhakaId, 0, 5, GETDATE()),
    (N'Tejgaon', N'তেজগাঁও', @DhakaId, 0, 6, GETDATE()),
    (N'Mohammadpur', N'মোহাম্মদপুর', @DhakaId, 0, 7, GETDATE()),
    (N'Dhanmondi', N'ধানমন্ডি', @DhakaId, 0, 8, GETDATE()),
    (N'Ramna', N'রমনা', @DhakaId, 0, 9, GETDATE()),
    (N'Motijheel', N'মতিঝিল', @DhakaId, 0, 10, GETDATE()),
    (N'Sabujbagh', N'সবুজবাগ', @DhakaId, 0, 11, GETDATE()),
    (N'Demra', N'ডেমরা', @DhakaId, 0, 12, GETDATE()),
    (N'Kotwali', N'কোতোয়ালি', @DhakaId, 0, 13, GETDATE()),
    (N'Sutrapur', N'সূত্রাপুর', @DhakaId, 0, 14, GETDATE()),
    (N'Lalbagh', N'লালবাগ', @DhakaId, 0, 15, GETDATE()),
    (N'Kamrangirchar', N'কামরাঙ্গীরচর', @DhakaId, 0, 16, GETDATE()),
    (N'Hazaribagh', N'হাজারীবাগ', @DhakaId, 0, 17, GETDATE()),
    (N'Gulshan', N'গুলশান', @DhakaId, 0, 18, GETDATE()),
    (N'Mirpur', N'মিরপুর', @DhakaId, 0, 19, GETDATE()),
    (N'Pallabi', N'পল্লবী', @DhakaId, 0, 20, GETDATE()),
    (N'Cantonment', N'ক্যান্টনমেন্ট', @DhakaId, 0, 21, GETDATE()),
    (N'Uttara', N'উত্তরা', @DhakaId, 0, 22, GETDATE()),
    (N'Dakshinkhan', N'দক্ষিণখান', @DhakaId, 0, 23, GETDATE());
    PRINT '  ✓ Dhaka: 23 upazilas inserted';
END
ELSE PRINT '  ✗ ERROR: Dhaka district not found!';

-- Faridpur District (9 upazilas)
DECLARE @FaridpurId INT = (SELECT TOP 1 Id FROM Districts WHERE Name = 'Faridpur' ORDER BY Id);
IF @FaridpurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Faridpur Sadar', N'ফরিদপুর সদর', @FaridpurId, 0, 1, GETDATE()),
    (N'Alfadanga', N'আলফাডাঙ্গা', @FaridpurId, 0, 2, GETDATE()),
    (N'Boalmari', N'বোয়ালমারী', @FaridpurId, 0, 3, GETDATE()),
    (N'Char Bhadrasan', N'চরভদ্রাসন', @FaridpurId, 0, 4, GETDATE()),
    (N'Madhukhali', N'মধুখালি', @FaridpurId, 0, 5, GETDATE()),
    (N'Nagarkanda', N'নগরকান্দা', @FaridpurId, 0, 6, GETDATE()),
    (N'Sadarpur', N'সদরপুর', @FaridpurId, 0, 7, GETDATE()),
    (N'Saltha', N'সালথা', @FaridpurId, 0, 8, GETDATE()),
    (N'Bhanga', N'ভাঙ্গা', @FaridpurId, 0, 9, GETDATE());
    PRINT '  ✓ Faridpur: 9 upazilas inserted';
END
ELSE PRINT '  ✗ ERROR: Faridpur district not found!';

-- Gazipur District (5 upazilas)
DECLARE @GazipurId INT = (SELECT TOP 1 Id FROM Districts WHERE Name = 'Gazipur' ORDER BY Id);
IF @GazipurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Gazipur Sadar', N'গাজীপুর সদর', @GazipurId, 0, 1, GETDATE()),
    (N'Kaliakair', N'কালিয়াকৈর', @GazipurId, 0, 2, GETDATE()),
    (N'Kapasia', N'কাপাসিয়া', @GazipurId, 0, 3, GETDATE()),
    (N'Sreepur', N'শ্রীপুর', @GazipurId, 0, 4, GETDATE()),
    (N'Kaliganj', N'কালীগঞ্জ', @GazipurId, 0, 5, GETDATE());
    PRINT '  ✓ Gazipur: 5 upazilas inserted';
END
ELSE PRINT '  ✗ ERROR: Gazipur district not found!';

-- Gopalganj District (5 upazilas)
DECLARE @GopalganjId INT = (SELECT TOP 1 Id FROM Districts WHERE Name = 'Gopalganj' ORDER BY Id);
IF @GopalganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Gopalganj Sadar', N'গোপালগঞ্জ সদর', @GopalganjId, 0, 1, GETDATE()),
    (N'Kashiani', N'কাশিয়ানী', @GopalganjId, 0, 2, GETDATE()),
    (N'Kotalipara', N'কোটালীপাড়া', @GopalganjId, 0, 3, GETDATE()),
    (N'Muksudpur', N'মুকসুদপুর', @GopalganjId, 0, 4, GETDATE()),
    (N'Tungipara', N'টুঙ্গীপাড়া', @GopalganjId, 0, 5, GETDATE());
    PRINT '  ✓ Gopalganj: 5 upazilas inserted';
END
ELSE PRINT '  ✗ ERROR: Gopalganj district not found!';

-- Kishoreganj District (13 upazilas)
DECLARE @KishoreganjId INT = (SELECT TOP 1 Id FROM Districts WHERE Name = 'Kishoreganj' ORDER BY Id);
IF @KishoreganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Kishoreganj Sadar', N'কিশোরগঞ্জ সদর', @KishoreganjId, 0, 1, GETDATE()),
    (N'Austagram', N'অষ্টগ্রাম', @KishoreganjId, 0, 2, GETDATE()),
    (N'Bajitpur', N'বাজিতপুর', @KishoreganjId, 0, 3, GETDATE()),
    (N'Bhairab', N'ভৈরব', @KishoreganjId, 0, 4, GETDATE()),
    (N'Hossainpur', N'হোসেনপুর', @KishoreganjId, 0, 5, GETDATE()),
    (N'Itna', N'ইটনা', @KishoreganjId, 0, 6, GETDATE()),
    (N'Karimganj', N'করিমগঞ্জ', @KishoreganjId, 0, 7, GETDATE()),
    (N'Katiadi', N'কটিয়াদী', @KishoreganjId, 0, 8, GETDATE()),
    (N'Kuliarchar', N'কুলিয়ারচর', @KishoreganjId, 0, 9, GETDATE()),
    (N'Mithamain', N'মিঠামইন', @KishoreganjId, 0, 10, GETDATE()),
    (N'Nikli', N'নিকলি', @KishoreganjId, 0, 11, GETDATE()),
    (N'Pakundia', N'পাকুন্ডিয়া', @KishoreganjId, 0, 12, GETDATE()),
    (N'Tarail', N'তাড়াইল', @KishoreganjId, 0, 13, GETDATE());
    PRINT '  ✓ Kishoreganj: 13 upazilas inserted';
END
ELSE PRINT '  ✗ ERROR: Kishoreganj district not found!';

-- Madaripur District (4 upazilas)
DECLARE @MadaripurId INT = (SELECT TOP 1 Id FROM Districts WHERE Name = 'Madaripur' ORDER BY Id);
IF @MadaripurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Madaripur Sadar', N'মাদারীপুর সদর', @MadaripurId, 0, 1, GETDATE()),
    (N'Kalkini', N'কালকিনি', @MadaripurId, 0, 2, GETDATE()),
    (N'Rajoir', N'রাজৈর', @MadaripurId, 0, 3, GETDATE()),
    (N'Shibchar', N'শিবচর', @MadaripurId, 0, 4, GETDATE());
    PRINT '  ✓ Madaripur: 4 upazilas inserted';
END
ELSE PRINT '  ✗ ERROR: Madaripur district not found!';

-- Manikganj District (7 upazilas)
DECLARE @ManikganjId INT = (SELECT TOP 1 Id FROM Districts WHERE Name = 'Manikganj' ORDER BY Id);
IF @ManikganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Manikganj Sadar', N'মানিকগঞ্জ সদর', @ManikganjId, 0, 1, GETDATE()),
    (N'Daulatpur', N'দৌলতপুর', @ManikganjId, 0, 2, GETDATE()),
    (N'Ghior', N'ঘিওর', @ManikganjId, 0, 3, GETDATE()),
    (N'Harirampur', N'হরিরামপুর', @ManikganjId, 0, 4, GETDATE()),
    (N'Saturia', N'সাটুরিয়া', @ManikganjId, 0, 5, GETDATE()),
    (N'Shivalaya', N'শিবালয়', @ManikganjId, 0, 6, GETDATE()),
    (N'Singair', N'সিংগাইর', @ManikganjId, 0, 7, GETDATE());
    PRINT '  ✓ Manikganj: 7 upazilas inserted';
END
ELSE PRINT '  ✗ ERROR: Manikganj district not found!';

-- Munshiganj District (6 upazilas)
DECLARE @MunshiganjId INT = (SELECT TOP 1 Id FROM Districts WHERE Name = 'Munshiganj' ORDER BY Id);
IF @MunshiganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Munshiganj Sadar', N'মুন্সীগঞ্জ সদর', @MunshiganjId, 0, 1, GETDATE()),
    (N'Gazaria', N'গজারিয়া', @MunshiganjId, 0, 2, GETDATE()),
    (N'Lohajang', N'লোহাজং', @MunshiganjId, 0, 3, GETDATE()),
    (N'Sirajdikhan', N'সিরাজদিখান', @MunshiganjId, 0, 4, GETDATE()),
    (N'Sreenagar', N'শ্রীনগর', @MunshiganjId, 0, 5, GETDATE()),
    (N'Tongibari', N'টংগীবাড়ী', @MunshiganjId, 0, 6, GETDATE());
    PRINT '  ✓ Munshiganj: 6 upazilas inserted';
END
ELSE PRINT '  ✗ ERROR: Munshiganj district not found!';

-- Narayanganj District (5 upazilas)
DECLARE @NarayanganjId INT = (SELECT TOP 1 Id FROM Districts WHERE Name = 'Narayanganj' ORDER BY Id);
IF @NarayanganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Narayanganj Sadar', N'নারায়ণগঞ্জ সদর', @NarayanganjId, 0, 1, GETDATE()),
    (N'Araihazar', N'আড়াইহাজার', @NarayanganjId, 0, 2, GETDATE()),
    (N'Bandar', N'বন্দর', @NarayanganjId, 0, 3, GETDATE()),
    (N'Rupganj', N'রূপগঞ্জ', @NarayanganjId, 0, 4, GETDATE()),
    (N'Sonargaon', N'সোনারগাঁ', @NarayanganjId, 0, 5, GETDATE());
    PRINT '  ✓ Narayanganj: 5 upazilas inserted';
END
ELSE PRINT '  ✗ ERROR: Narayanganj district not found!';

-- Narsingdi District (6 upazilas)
DECLARE @NarsingdiId INT = (SELECT TOP 1 Id FROM Districts WHERE Name = 'Narsingdi' ORDER BY Id);
IF @NarsingdiId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Narsingdi Sadar', N'নরসিংদী সদর', @NarsingdiId, 0, 1, GETDATE()),
    (N'Belabo', N'বেলাবো', @NarsingdiId, 0, 2, GETDATE()),
    (N'Monohardi', N'মনোহরদী', @NarsingdiId, 0, 3, GETDATE()),
    (N'Palash', N'পলাশ', @NarsingdiId, 0, 4, GETDATE()),
    (N'Raipura', N'রায়পুরা', @NarsingdiId, 0, 5, GETDATE()),
    (N'Shibpur', N'শিবপুর', @NarsingdiId, 0, 6, GETDATE());
    PRINT '  ✓ Narsingdi: 6 upazilas inserted';
END
ELSE PRINT '  ✗ ERROR: Narsingdi district not found!';

-- Rajbari District (5 upazilas)
DECLARE @RajbariId INT = (SELECT TOP 1 Id FROM Districts WHERE Name = 'Rajbari' ORDER BY Id);
IF @RajbariId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Rajbari Sadar', N'রাজবাড়ী সদর', @RajbariId, 0, 1, GETDATE()),
    (N'Baliakandi', N'বালিয়াকান্দি', @RajbariId, 0, 2, GETDATE()),
    (N'Goalandaghat', N'গোয়ালন্দঘাট', @RajbariId, 0, 3, GETDATE()),
    (N'Kalukhali', N'কালুখালি', @RajbariId, 0, 4, GETDATE()),
    (N'Pangsha', N'পাংশা', @RajbariId, 0, 5, GETDATE());
    PRINT '  ✓ Rajbari: 5 upazilas inserted';
END
ELSE PRINT '  ✗ ERROR: Rajbari district not found!';

-- Shariatpur District (7 upazilas)
DECLARE @ShariatpurId INT = (SELECT TOP 1 Id FROM Districts WHERE Name = 'Shariatpur' ORDER BY Id);
IF @ShariatpurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Shariatpur Sadar', N'শরীয়তপুর সদর', @ShariatpurId, 0, 1, GETDATE()),
    (N'Bhedarganj', N'ভেদরগঞ্জ', @ShariatpurId, 0, 2, GETDATE()),
    (N'Damudya', N'ডামুড্যা', @ShariatpurId, 0, 3, GETDATE()),
    (N'Gosairhat', N'গোসাইরহাট', @ShariatpurId, 0, 4, GETDATE()),
    (N'Naria', N'নড়িয়া', @ShariatpurId, 0, 5, GETDATE()),
    (N'Zajira', N'জাজিরা', @ShariatpurId, 0, 6, GETDATE()),
    (N'Shakhipur', N'শখিপুর', @ShariatpurId, 0, 7, GETDATE());
    PRINT '  ✓ Shariatpur: 7 upazilas inserted';
END
ELSE PRINT '  ✗ ERROR: Shariatpur district not found!';

-- Tangail District (12 upazilas)
DECLARE @TangailId INT = (SELECT TOP 1 Id FROM Districts WHERE Name = 'Tangail' ORDER BY Id);
IF @TangailId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Tangail Sadar', N'টাঙ্গাইল সদর', @TangailId, 0, 1, GETDATE()),
    (N'Basail', N'বাসাইল', @TangailId, 0, 2, GETDATE()),
    (N'Bhuapur', N'ভুয়াপুর', @TangailId, 0, 3, GETDATE()),
    (N'Delduar', N'দেলদুয়ার', @TangailId, 0, 4, GETDATE()),
    (N'Dhanbari', N'ধনবাড়ী', @TangailId, 0, 5, GETDATE()),
    (N'Ghatail', N'ঘাটাইল', @TangailId, 0, 6, GETDATE()),
    (N'Gopalpur', N'গোপালপুর', @TangailId, 0, 7, GETDATE()),
    (N'Kalihati', N'কালিহাতী', @TangailId, 0, 8, GETDATE()),
    (N'Madhupur', N'মধুপুর', @TangailId, 0, 9, GETDATE()),
    (N'Mirzapur', N'মির্জাপুর', @TangailId, 0, 10, GETDATE()),
    (N'Nagarpur', N'নাগরপুর', @TangailId, 0, 11, GETDATE()),
    (N'Sakhipur', N'সখীপুর', @TangailId, 0, 12, GETDATE());
    PRINT '  ✓ Tangail: 12 upazilas inserted';
END
ELSE PRINT '  ✗ ERROR: Tangail district not found!';

PRINT '';
PRINT 'Dhaka Division completed!';
PRINT '';
PRINT '=================================================================';
PRINT 'Part 1 (Dhaka Division) completed successfully!';
PRINT 'Total upazilas inserted: 100+ (Dhaka Division only)';
PRINT '';
PRINT 'Next: Run BangladeshUpazilasComplete_Part2.sql for remaining divisions';
PRINT '=================================================================';
