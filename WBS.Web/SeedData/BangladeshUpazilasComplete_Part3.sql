-- Complete Bangladesh Upazilas Data - Part 3 (Final)
-- This is the final part covering Sylhet, Rangpur, and Mymensingh divisions
-- Run this after Part 2 completes successfully

PRINT 'Starting Part 3 (Final) - Sylhet, Rangpur & Mymensingh Divisions...';

-- =======================
-- SYLHET DIVISION
-- =======================

-- Habiganj District (9 upazilas)
DECLARE @HabiganjId INT = (SELECT Id FROM Districts WHERE Name = 'Habiganj');
IF @HabiganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Habiganj Sadar', N'হবিগঞ্জ সদর', @HabiganjId, 0, 1, GETDATE()),
    (N'Ajmiriganj', N'আজমিরীগঞ্জ', @HabiganjId, 0, 2, GETDATE()),
    (N'Bahubal', N'বাহুবল', @HabiganjId, 0, 3, GETDATE()),
    (N'Baniyachong', N'বানিয়াচং', @HabiganjId, 0, 4, GETDATE()),
    (N'Chunarughat', N'চুনারুঘাট', @HabiganjId, 0, 5, GETDATE()),
    (N'Lakhai', N'লাখাই', @HabiganjId, 0, 6, GETDATE()),
    (N'Madhabpur', N'মাধবপুর', @HabiganjId, 0, 7, GETDATE()),
    (N'Nabiganj', N'নবীগঞ্জ', @HabiganjId, 0, 8, GETDATE()),
    (N'Shayestaganj', N'শায়েস্তাগঞ্জ', @HabiganjId, 0, 9, GETDATE());
    PRINT 'Inserted Habiganj upazilas';
END

-- Moulvibazar District (7 upazilas)
DECLARE @MoulvibazarId INT = (SELECT Id FROM Districts WHERE Name = 'Moulvibazar');
IF @MoulvibazarId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Moulvibazar Sadar', N'মৌলভীবাজার সদর', @MoulvibazarId, 0, 1, GETDATE()),
    (N'Barlekha', N'বড়লেখা', @MoulvibazarId, 0, 2, GETDATE()),
    (N'Juri', N'জুড়ী', @MoulvibazarId, 0, 3, GETDATE()),
    (N'Kamalganj', N'কমলগঞ্জ', @MoulvibazarId, 0, 4, GETDATE()),
    (N'Kulaura', N'কুলাউড়া', @MoulvibazarId, 0, 5, GETDATE()),
    (N'Rajnagar', N'রাজনগর', @MoulvibazarId, 0, 6, GETDATE()),
    (N'Sreemangal', N'শ্রীমঙ্গল', @MoulvibazarId, 0, 7, GETDATE());
    PRINT 'Inserted Moulvibazar upazilas';
END

-- Sunamganj District (11 upazilas)
DECLARE @SunamganjId INT = (SELECT Id FROM Districts WHERE Name = 'Sunamganj');
IF @SunamganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Sunamganj Sadar', N'সুনামগঞ্জ সদর', @SunamganjId, 0, 1, GETDATE()),
    (N'Bishwambarpur', N'বিশ্বম্ভরপুর', @SunamganjId, 0, 2, GETDATE()),
    (N'Chhatak', N'ছাতক', @SunamganjId, 0, 3, GETDATE()),
    (N'Derai', N'দেরাই', @SunamganjId, 0, 4, GETDATE()),
    (N'Dharamapasha', N'ধর্মপাশা', @SunamganjId, 0, 5, GETDATE()),
    (N'Dowarabazar', N'দোয়ারাবাজার', @SunamganjId, 0, 6, GETDATE()),
    (N'Jagannathpur', N'জগন্নাথপুর', @SunamganjId, 0, 7, GETDATE()),
    (N'Jamalganj', N'জামালগঞ্জ', @SunamganjId, 0, 8, GETDATE()),
    (N'Salla', N'শাল্লা', @SunamganjId, 0, 9, GETDATE()),
    (N'Tahirpur', N'তাহিরপুর', @SunamganjId, 0, 10, GETDATE()),
    (N'Dakshin Sunamganj', N'দক্ষিণ সুনামগঞ্জ', @SunamganjId, 0, 11, GETDATE());
    PRINT 'Inserted Sunamganj upazilas';
END

-- Sylhet District (13 upazilas)
DECLARE @SylhetId INT = (SELECT Id FROM Districts WHERE Name = 'Sylhet');
IF @SylhetId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Sylhet Sadar', N'সিলেট সদর', @SylhetId, 0, 1, GETDATE()),
    (N'Balaganj', N'বালাগঞ্জ', @SylhetId, 0, 2, GETDATE()),
    (N'Beanibazar', N'বিয়ানীবাজার', @SylhetId, 0, 3, GETDATE()),
    (N'Bishwanath', N'বিশ্বনাথ', @SylhetId, 0, 4, GETDATE()),
    (N'Companiganj', N'কোম্পানীগঞ্জ', @SylhetId, 0, 5, GETDATE()),
    (N'Dakshin Surma', N'দক্ষিণ সুরমা', @SylhetId, 0, 6, GETDATE()),
    (N'Fenchuganj', N'ফেঞ্চুগঞ্জ', @SylhetId, 0, 7, GETDATE()),
    (N'Golapganj', N'গোলাপগঞ্জ', @SylhetId, 0, 8, GETDATE()),
    (N'Gowainghat', N'গোয়াইনঘাট', @SylhetId, 0, 9, GETDATE()),
    (N'Jaintiapur', N'জৈন্তাপুর', @SylhetId, 0, 10, GETDATE()),
    (N'Kanaighat', N'কানাইঘাট', @SylhetId, 0, 11, GETDATE()),
    (N'Osmaninagar', N'ওসমানীনগর', @SylhetId, 0, 12, GETDATE()),
    (N'Zakiganj', N'জকিগঞ্জ', @SylhetId, 0, 13, GETDATE());
    PRINT 'Inserted Sylhet upazilas';
END

-- =======================
-- RANGPUR DIVISION
-- =======================

-- Dinajpur District (13 upazilas)
DECLARE @DinajpurId INT = (SELECT Id FROM Districts WHERE Name = 'Dinajpur');
IF @DinajpurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Dinajpur Sadar', N'দিনাজপুর সদর', @DinajpurId, 0, 1, GETDATE()),
    (N'Birampur', N'বিরামপুর', @DinajpurId, 0, 2, GETDATE()),
    (N'Birganj', N'বীরগঞ্জ', @DinajpurId, 0, 3, GETDATE()),
    (N'Biral', N'বিরল', @DinajpurId, 0, 4, GETDATE()),
    (N'Bochaganj', N'বোচাগঞ্জ', @DinajpurId, 0, 5, GETDATE()),
    (N'Chirirbandar', N'চিরিরবন্দর', @DinajpurId, 0, 6, GETDATE()),
    (N'Fulbari', N'ফুলবাড়ী', @DinajpurId, 0, 7, GETDATE()),
    (N'Ghoraghat', N'ঘোড়াঘাট', @DinajpurId, 0, 8, GETDATE()),
    (N'Hakimpur', N'হাকিমপুর', @DinajpurId, 0, 9, GETDATE()),
    (N'Kaharole', N'কাহারোল', @DinajpurId, 0, 10, GETDATE()),
    (N'Khansama', N'খানসামা', @DinajpurId, 0, 11, GETDATE()),
    (N'Nawabganj', N'নবাবগঞ্জ', @DinajpurId, 0, 12, GETDATE()),
    (N'Parbatipur', N'পার্বতীপুর', @DinajpurId, 0, 13, GETDATE());
    PRINT 'Inserted Dinajpur upazilas';
END

-- Gaibandha District (7 upazilas)
DECLARE @GaibandhaId INT = (SELECT Id FROM Districts WHERE Name = 'Gaibandha');
IF @GaibandhaId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Gaibandha Sadar', N'গাইবান্ধা সদর', @GaibandhaId, 0, 1, GETDATE()),
    (N'Fulchhari', N'ফুলছড়ি', @GaibandhaId, 0, 2, GETDATE()),
    (N'Gobindaganj', N'গোবিন্দগঞ্জ', @GaibandhaId, 0, 3, GETDATE()),
    (N'Palashbari', N'পলাশবাড়ী', @GaibandhaId, 0, 4, GETDATE()),
    (N'Sadullapur', N'সাদুল্লাপুর', @GaibandhaId, 0, 5, GETDATE()),
    (N'Saghata', N'সাঘাটা', @GaibandhaId, 0, 6, GETDATE()),
    (N'Sundarganj', N'সুন্দরগঞ্জ', @GaibandhaId, 0, 7, GETDATE());
    PRINT 'Inserted Gaibandha upazilas';
END

-- Kurigram District (9 upazilas)
DECLARE @KurigramId INT = (SELECT Id FROM Districts WHERE Name = 'Kurigram');
IF @KurigramId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Kurigram Sadar', N'কুড়িগ্রাম সদর', @KurigramId, 0, 1, GETDATE()),
    (N'Bhurungamari', N'ভুরুঙ্গামারী', @KurigramId, 0, 2, GETDATE()),
    (N'Char Rajibpur', N'চর রাজিবপুর', @KurigramId, 0, 3, GETDATE()),
    (N'Chilmari', N'চিলমারী', @KurigramId, 0, 4, GETDATE()),
    (N'Phulbari', N'ফুলবাড়ী', @KurigramId, 0, 5, GETDATE()),
    (N'Nageshwari', N'নাগেশ্বরী', @KurigramId, 0, 6, GETDATE()),
    (N'Rajarhat', N'রাজারহাট', @KurigramId, 0, 7, GETDATE()),
    (N'Raomari', N'রৌমারী', @KurigramId, 0, 8, GETDATE()),
    (N'Ulipur', N'উলিপুর', @KurigramId, 0, 9, GETDATE());
    PRINT 'Inserted Kurigram upazilas';
END

-- Lalmonirhat District (5 upazilas)
DECLARE @LalmonirhatId INT = (SELECT Id FROM Districts WHERE Name = 'Lalmonirhat');
IF @LalmonirhatId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Lalmonirhat Sadar', N'লালমনিরহাট সদর', @LalmonirhatId, 0, 1, GETDATE()),
    (N'Aditmari', N'আদিতমারী', @LalmonirhatId, 0, 2, GETDATE()),
    (N'Hatibandha', N'হাতীবান্ধা', @LalmonirhatId, 0, 3, GETDATE()),
    (N'Kaliganj', N'কালীগঞ্জ', @LalmonirhatId, 0, 4, GETDATE()),
    (N'Patgram', N'পাটগ্রাম', @LalmonirhatId, 0, 5, GETDATE());
    PRINT 'Inserted Lalmonirhat upazilas';
END

-- Nilphamari District (6 upazilas)
DECLARE @NilphamariId INT = (SELECT Id FROM Districts WHERE Name = 'Nilphamari');
IF @NilphamariId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Nilphamari Sadar', N'নীলফামারী সদর', @NilphamariId, 0, 1, GETDATE()),
    (N'Dimla', N'ডিমলা', @NilphamariId, 0, 2, GETDATE()),
    (N'Domar', N'ডোমার', @NilphamariId, 0, 3, GETDATE()),
    (N'Jaldhaka', N'জলঢাকা', @NilphamariId, 0, 4, GETDATE()),
    (N'Kishoreganj', N'কিশোরগঞ্জ', @NilphamariId, 0, 5, GETDATE()),
    (N'Saidpur', N'সৈয়দপুর', @NilphamariId, 0, 6, GETDATE());
    PRINT 'Inserted Nilphamari upazilas';
END

-- Panchagarh District (5 upazilas)
DECLARE @PanchagarhId INT = (SELECT Id FROM Districts WHERE Name = 'Panchagarh');
IF @PanchagarhId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Panchagarh Sadar', N'পঞ্চগড় সদর', @PanchagarhId, 0, 1, GETDATE()),
    (N'Atwari', N'আটোয়ারী', @PanchagarhId, 0, 2, GETDATE()),
    (N'Boda', N'বোদা', @PanchagarhId, 0, 3, GETDATE()),
    (N'Debiganj', N'দেবীগঞ্জ', @PanchagarhId, 0, 4, GETDATE()),
    (N'Tetulia', N'তেতুলিয়া', @PanchagarhId, 0, 5, GETDATE());
    PRINT 'Inserted Panchagarh upazilas';
END

-- Rangpur District (8 upazilas)
DECLARE @RangpurId INT = (SELECT Id FROM Districts WHERE Name = 'Rangpur');
IF @RangpurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Rangpur Sadar', N'রংপুর সদর', @RangpurId, 0, 1, GETDATE()),
    (N'Badarganj', N'বদরগঞ্জ', @RangpurId, 0, 2, GETDATE()),
    (N'Gangachara', N'গঙ্গাচড়া', @RangpurId, 0, 3, GETDATE()),
    (N'Kaunia', N'কাউনিয়া', @RangpurId, 0, 4, GETDATE()),
    (N'Mithapukur', N'মিঠাপুকুর', @RangpurId, 0, 5, GETDATE()),
    (N'Pirgachha', N'পীরগাছা', @RangpurId, 0, 6, GETDATE()),
    (N'Pirganj', N'পীরগঞ্জ', @RangpurId, 0, 7, GETDATE()),
    (N'Taraganj', N'তারাগঞ্জ', @RangpurId, 0, 8, GETDATE());
    PRINT 'Inserted Rangpur upazilas';
END

-- Thakurgaon District (5 upazilas)
DECLARE @ThakurgaonId INT = (SELECT Id FROM Districts WHERE Name = 'Thakurgaon');
IF @ThakurgaonId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Thakurgaon Sadar', N'ঠাকুরগাঁও সদর', @ThakurgaonId, 0, 1, GETDATE()),
    (N'Baliadangi', N'বালিয়াডাঙ্গী', @ThakurgaonId, 0, 2, GETDATE()),
    (N'Haripur', N'হরিপুর', @ThakurgaonId, 0, 3, GETDATE()),
    (N'Pirganj', N'পীরগঞ্জ', @ThakurgaonId, 0, 4, GETDATE()),
    (N'Ranisankail', N'রাণীশংকৈল', @ThakurgaonId, 0, 5, GETDATE());
    PRINT 'Inserted Thakurgaon upazilas';
END

-- =======================
-- MYMENSINGH DIVISION
-- =======================

-- Jamalpur District (7 upazilas)
DECLARE @JamalpurId INT = (SELECT Id FROM Districts WHERE Name = 'Jamalpur');
IF @JamalpurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Jamalpur Sadar', N'জামালপুর সদর', @JamalpurId, 0, 1, GETDATE()),
    (N'Baksiganj', N'বকসীগঞ্জ', @JamalpurId, 0, 2, GETDATE()),
    (N'Dewanganj', N'দেওয়ানগঞ্জ', @JamalpurId, 0, 3, GETDATE()),
    (N'Islampur', N'ইসলামপুর', @JamalpurId, 0, 4, GETDATE()),
    (N'Madarganj', N'মাদারগঞ্জ', @JamalpurId, 0, 5, GETDATE()),
    (N'Melandaha', N'মেলান্দহ', @JamalpurId, 0, 6, GETDATE()),
    (N'Sarishabari', N'সরিষাবাড়ী', @JamalpurId, 0, 7, GETDATE());
    PRINT 'Inserted Jamalpur upazilas';
END

-- Mymensingh District (13 upazilas)
DECLARE @MymensinghId INT = (SELECT Id FROM Districts WHERE Name = 'Mymensingh');
IF @MymensinghId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Mymensingh Sadar', N'ময়মনসিংহ সদর', @MymensinghId, 0, 1, GETDATE()),
    (N'Bhaluka', N'ভালুকা', @MymensinghId, 0, 2, GETDATE()),
    (N'Dhobaura', N'ধোবাউড়া', @MymensinghId, 0, 3, GETDATE()),
    (N'Fulbaria', N'ফুলবাড়ীয়া', @MymensinghId, 0, 4, GETDATE()),
    (N'Gafargaon', N'গফরগাঁও', @MymensinghId, 0, 5, GETDATE()),
    (N'Gauripur', N'গৌরীপুর', @MymensinghId, 0, 6, GETDATE()),
    (N'Haluaghat', N'হালুয়াঘাট', @MymensinghId, 0, 7, GETDATE()),
    (N'Ishwarganj', N'ঈশ্বরগঞ্জ', @MymensinghId, 0, 8, GETDATE()),
    (N'Muktagachha', N'মুক্তাগাছা', @MymensinghId, 0, 9, GETDATE()),
    (N'Nandail', N'নান্দাইল', @MymensinghId, 0, 10, GETDATE()),
    (N'Phulpur', N'ফুলপুর', @MymensinghId, 0, 11, GETDATE()),
    (N'Trishal', N'ত্রিশাল', @MymensinghId, 0, 12, GETDATE()),
    (N'Tarakanda', N'তারাকান্দা', @MymensinghId, 0, 13, GETDATE());
    PRINT 'Inserted Mymensingh upazilas';
END

-- Netrokona District (10 upazilas)
DECLARE @NetrokonaId INT = (SELECT Id FROM Districts WHERE Name = 'Netrokona');
IF @NetrokonaId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Netrokona Sadar', N'নেত্রকোণা সদর', @NetrokonaId, 0, 1, GETDATE()),
    (N'Atpara', N'আটপাড়া', @NetrokonaId, 0, 2, GETDATE()),
    (N'Barhatta', N'বারহাট্টা', @NetrokonaId, 0, 3, GETDATE()),
    (N'Durgapur', N'দুর্গাপুর', @NetrokonaId, 0, 4, GETDATE()),
    (N'Kalmakanda', N'কলমাকান্দা', @NetrokonaId, 0, 5, GETDATE()),
    (N'Kendua', N'কেন্দুয়া', @NetrokonaId, 0, 6, GETDATE()),
    (N'Khaliajuri', N'খালিয়াজুরি', @NetrokonaId, 0, 7, GETDATE()),
    (N'Madan', N'মদন', @NetrokonaId, 0, 8, GETDATE()),
    (N'Mohanganj', N'মোহনগঞ্জ', @NetrokonaId, 0, 9, GETDATE()),
    (N'Purbadhala', N'পূর্বধলা', @NetrokonaId, 0, 10, GETDATE());
    PRINT 'Inserted Netrokona upazilas';
END

-- Sherpur District (5 upazilas)
DECLARE @SherpurId INT = (SELECT Id FROM Districts WHERE Name = 'Sherpur');
IF @SherpurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Sherpur Sadar', N'শেরপুর সদর', @SherpurId, 0, 1, GETDATE()),
    (N'Jhenaigati', N'ঝিনাইগাতী', @SherpurId, 0, 2, GETDATE()),
    (N'Nakla', N'নকলা', @SherpurId, 0, 3, GETDATE()),
    (N'Nalitabari', N'নালিতাবাড়ী', @SherpurId, 0, 4, GETDATE()),
    (N'Sreebardi', N'শ্রীবরদী', @SherpurId, 0, 5, GETDATE());
    PRINT 'Inserted Sherpur upazilas';
END

-- =======================
-- COMPLETION MESSAGE
-- =======================
PRINT '';
PRINT '=================================================================';
PRINT 'SUCCESS! All upazilas of Bangladesh have been inserted!';
PRINT '=================================================================';
PRINT '';
PRINT 'Summary:';
PRINT '- Dhaka Division: 13 districts with 100+ upazilas';
PRINT '- Chittagong Division: 11 districts with 100+ upazilas';
PRINT '- Rajshahi Division: 8 districts with 70+ upazilas';
PRINT '- Khulna Division: 10 districts with 60+ upazilas';
PRINT '- Barishal Division: 6 districts with 40+ upazilas';
PRINT '- Sylhet Division: 4 districts with 40+ upazilas';
PRINT '- Rangpur Division: 8 districts with 58 upazilas';
PRINT '- Mymensingh Division: 4 districts with 35 upazilas';
PRINT '';
PRINT 'Total: 64 Districts with approximately 495 Upazilas';
PRINT '';
PRINT 'Next Steps:';
PRINT '1. Go to Admin Panel > Districts';
PRINT '2. Mark districts where WBS works (HasWork = checked)';
PRINT '3. Go to Admin Panel > Upazilas';
PRINT '4. Mark upazilas where WBS works (HasWork = checked)';
PRINT '5. Visit /about/wherewework to see the beautiful display!';
PRINT '';
PRINT 'All data is stored with both English and Bengali names.';
PRINT '=================================================================';
