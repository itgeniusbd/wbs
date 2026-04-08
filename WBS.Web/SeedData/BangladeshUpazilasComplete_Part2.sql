-- Complete Bangladesh Upazilas Data - Part 2
-- This is continuation of Part 1
-- Run this after Part 1 completes successfully

PRINT 'Starting Part 2 - Remaining Divisions...';

-- =======================
-- RAJSHAHI DIVISION
-- =======================

-- Bogura District (12 upazilas)
DECLARE @BoguraId INT = (SELECT Id FROM Districts WHERE Name = 'Bogura');
IF @BoguraId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Bogura Sadar', N'বগুড়া সদর', @BoguraId, 0, 1, GETDATE()),
    (N'Adamdighi', N'আদমদিঘী', @BoguraId, 0, 2, GETDATE()),
    (N'Dhunat', N'ধুনট', @BoguraId, 0, 3, GETDATE()),
    (N'Dupchanchia', N'দুপচাঁচিয়া', @BoguraId, 0, 4, GETDATE()),
    (N'Gabtali', N'গাবতলী', @BoguraId, 0, 5, GETDATE()),
    (N'Kahaloo', N'কাহালু', @BoguraId, 0, 6, GETDATE()),
    (N'Nandigram', N'নন্দিগ্রাম', @BoguraId, 0, 7, GETDATE()),
    (N'Sariakandi', N'সারিয়াকান্দি', @BoguraId, 0, 8, GETDATE()),
    (N'Shajahanpur', N'শাজাহানপুর', @BoguraId, 0, 9, GETDATE()),
    (N'Sherpur', N'শেরপুর', @BoguraId, 0, 10, GETDATE()),
    (N'Shibganj', N'শিবগঞ্জ', @BoguraId, 0, 11, GETDATE()),
    (N'Sonatola', N'সোনাতলা', @BoguraId, 0, 12, GETDATE());
    PRINT 'Inserted Bogura upazilas';
END

-- Joypurhat District (5 upazilas)
DECLARE @JoypurhatId INT = (SELECT Id FROM Districts WHERE Name = 'Joypurhat');
IF @JoypurhatId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Joypurhat Sadar', N'জয়পুরহাট সদর', @JoypurhatId, 0, 1, GETDATE()),
    (N'Akkelpur', N'আক্কেলপুর', @JoypurhatId, 0, 2, GETDATE()),
    (N'Kalai', N'কালাই', @JoypurhatId, 0, 3, GETDATE()),
    (N'Khetlal', N'ক্ষেতলাল', @JoypurhatId, 0, 4, GETDATE()),
    (N'Panchbibi', N'পাঁচবিবি', @JoypurhatId, 0, 5, GETDATE());
    PRINT 'Inserted Joypurhat upazilas';
END

-- Naogaon District (11 upazilas)
DECLARE @NaogaonId INT = (SELECT Id FROM Districts WHERE Name = 'Naogaon');
IF @NaogaonId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Naogaon Sadar', N'নওগাঁ সদর', @NaogaonId, 0, 1, GETDATE()),
    (N'Atrai', N'আত্রাই', @NaogaonId, 0, 2, GETDATE()),
    (N'Badalgachhi', N'বদলগাছী', @NaogaonId, 0, 3, GETDATE()),
    (N'Dhamoirhat', N'ধামইরহাট', @NaogaonId, 0, 4, GETDATE()),
    (N'Manda', N'মান্দা', @NaogaonId, 0, 5, GETDATE()),
    (N'Mahadebpur', N'মহাদেবপুর', @NaogaonId, 0, 6, GETDATE()),
    (N'Niamatpur', N'নিয়ামতপুর', @NaogaonId, 0, 7, GETDATE()),
    (N'Patnitala', N'পত্নীতলা', @NaogaonId, 0, 8, GETDATE()),
    (N'Porsha', N'পোরশা', @NaogaonId, 0, 9, GETDATE()),
    (N'Raninagar', N'রাণীনগর', @NaogaonId, 0, 10, GETDATE()),
    (N'Sapahar', N'সাপাহার', @NaogaonId, 0, 11, GETDATE());
    PRINT 'Inserted Naogaon upazilas';
END

-- Natore District (7 upazilas)
DECLARE @NatoreId INT = (SELECT Id FROM Districts WHERE Name = 'Natore');
IF @NatoreId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Natore Sadar', N'নাটোর সদর', @NatoreId, 0, 1, GETDATE()),
    (N'Bagatipara', N'বাগাতিপাড়া', @NatoreId, 0, 2, GETDATE()),
    (N'Baraigram', N'বড়াইগ্রাম', @NatoreId, 0, 3, GETDATE()),
    (N'Gurudaspur', N'গুরুদাসপুর', @NatoreId, 0, 4, GETDATE()),
    (N'Lalpur', N'লালপুর', @NatoreId, 0, 5, GETDATE()),
    (N'Naldanga', N'নলডাঙ্গা', @NatoreId, 0, 6, GETDATE()),
    (N'Singra', N'সিংড়া', @NatoreId, 0, 7, GETDATE());
    PRINT 'Inserted Natore upazilas';
END

-- Chapainawabganj District (5 upazilas)
DECLARE @ChapainawabganjId INT = (SELECT Id FROM Districts WHERE Name = 'Chapainawabganj');
IF @ChapainawabganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Chapainawabganj Sadar', N'চাঁপাইনবাবগঞ্জ সদর', @ChapainawabganjId, 0, 1, GETDATE()),
    (N'Bholahat', N'ভোলাহাট', @ChapainawabganjId, 0, 2, GETDATE()),
    (N'Gomastapur', N'গোমস্তাপুর', @ChapainawabganjId, 0, 3, GETDATE()),
    (N'Nachole', N'নাচোল', @ChapainawabganjId, 0, 4, GETDATE()),
    (N'Shibganj', N'শিবগঞ্জ', @ChapainawabganjId, 0, 5, GETDATE());
    PRINT 'Inserted Chapainawabganj upazilas';
END

-- Pabna District (9 upazilas)
DECLARE @PabnaId INT = (SELECT Id FROM Districts WHERE Name = 'Pabna');
IF @PabnaId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Pabna Sadar', N'পাবনা সদর', @PabnaId, 0, 1, GETDATE()),
    (N'Atgharia', N'আটঘরিয়া', @PabnaId, 0, 2, GETDATE()),
    (N'Bera', N'বেড়া', @PabnaId, 0, 3, GETDATE()),
    (N'Bhangura', N'ভাঙ্গুড়া', @PabnaId, 0, 4, GETDATE()),
    (N'Chatmohar', N'চাটমোহর', @PabnaId, 0, 5, GETDATE()),
    (N'Faridpur', N'ফরিদপুর', @PabnaId, 0, 6, GETDATE()),
    (N'Ishwardi', N'ঈশ্বরদী', @PabnaId, 0, 7, GETDATE()),
    (N'Santhia', N'সাঁথিয়া', @PabnaId, 0, 8, GETDATE()),
    (N'Sujanagar', N'সুজানগর', @PabnaId, 0, 9, GETDATE());
    PRINT 'Inserted Pabna upazilas';
END

-- Rajshahi District (9 upazilas)
DECLARE @RajshahiId INT = (SELECT Id FROM Districts WHERE Name = 'Rajshahi');
IF @RajshahiId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Rajshahi Sadar', N'রাজশাহী সদর', @RajshahiId, 0, 1, GETDATE()),
    (N'Bagha', N'বাঘা', @RajshahiId, 0, 2, GETDATE()),
    (N'Bagmara', N'বাগমারা', @RajshahiId, 0, 3, GETDATE()),
    (N'Charghat', N'চারঘাট', @RajshahiId, 0, 4, GETDATE()),
    (N'Durgapur', N'দুর্গাপুর', @RajshahiId, 0, 5, GETDATE()),
    (N'Godagari', N'গোদাগাড়ী', @RajshahiId, 0, 6, GETDATE()),
    (N'Mohanpur', N'মোহনপুর', @RajshahiId, 0, 7, GETDATE()),
    (N'Paba', N'পবা', @RajshahiId, 0, 8, GETDATE()),
    (N'Puthia', N'পুঠিয়া', @RajshahiId, 0, 9, GETDATE());
    PRINT 'Inserted Rajshahi upazilas';
END

-- Sirajganj District (9 upazilas)
DECLARE @SirajganjId INT = (SELECT Id FROM Districts WHERE Name = 'Sirajganj');
IF @SirajganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Sirajganj Sadar', N'সিরাজগঞ্জ সদর', @SirajganjId, 0, 1, GETDATE()),
    (N'Belkuchi', N'বেলকুচি', @SirajganjId, 0, 2, GETDATE()),
    (N'Chauhali', N'চৌহালি', @SirajganjId, 0, 3, GETDATE()),
    (N'Kamarkhanda', N'কামারখন্দ', @SirajganjId, 0, 4, GETDATE()),
    (N'Kazipur', N'কাজীপুর', @SirajganjId, 0, 5, GETDATE()),
    (N'Raiganj', N'রায়গঞ্জ', @SirajganjId, 0, 6, GETDATE()),
    (N'Shahjadpur', N'শাহজাদপুর', @SirajganjId, 0, 7, GETDATE()),
    (N'Tarash', N'তাড়াশ', @SirajganjId, 0, 8, GETDATE()),
    (N'Ullahpara', N'উল্লাপাড়া', @SirajganjId, 0, 9, GETDATE());
    PRINT 'Inserted Sirajganj upazilas';
END

-- =======================
-- KHULNA DIVISION
-- =======================

-- Bagerhat District (9 upazilas)
DECLARE @BagerhatId INT = (SELECT Id FROM Districts WHERE Name = 'Bagerhat');
IF @BagerhatId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Bagerhat Sadar', N'বাগেরহাট সদর', @BagerhatId, 0, 1, GETDATE()),
    (N'Chitalmari', N'চিতলমারী', @BagerhatId, 0, 2, GETDATE()),
    (N'Fakirhat', N'ফকিরহাট', @BagerhatId, 0, 3, GETDATE()),
    (N'Kachua', N'কচুয়া', @BagerhatId, 0, 4, GETDATE()),
    (N'Mollahat', N'মোল্লাহাট', @BagerhatId, 0, 5, GETDATE()),
    (N'Mongla', N'মংলা', @BagerhatId, 0, 6, GETDATE()),
    (N'Morrelganj', N'মোড়েলগঞ্জ', @BagerhatId, 0, 7, GETDATE()),
    (N'Rampal', N'রামপাল', @BagerhatId, 0, 8, GETDATE()),
    (N'Sarankhola', N'শরনখোলা', @BagerhatId, 0, 9, GETDATE());
    PRINT 'Inserted Bagerhat upazilas';
END

-- Chuadanga District (4 upazilas)
DECLARE @ChuadangaId INT = (SELECT Id FROM Districts WHERE Name = 'Chuadanga');
IF @ChuadangaId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Chuadanga Sadar', N'চুয়াডাঙ্গা সদর', @ChuadangaId, 0, 1, GETDATE()),
    (N'Alamdanga', N'আলমডাঙ্গা', @ChuadangaId, 0, 2, GETDATE()),
    (N'Damurhuda', N'দামুড়হুদা', @ChuadangaId, 0, 3, GETDATE()),
    (N'Jibannagar', N'জীবননগর', @ChuadangaId, 0, 4, GETDATE());
    PRINT 'Inserted Chuadanga upazilas';
END

-- Jashore District (8 upazilas)
DECLARE @JashoreId INT = (SELECT Id FROM Districts WHERE Name = 'Jashore');
IF @JashoreId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Jashore Sadar', N'যশোর সদর', @JashoreId, 0, 1, GETDATE()),
    (N'Abhaynagar', N'অভয়নগর', @JashoreId, 0, 2, GETDATE()),
    (N'Bagherpara', N'বাঘারপাড়া', @JashoreId, 0, 3, GETDATE()),
    (N'Chaugachha', N'চৌগাছা', @JashoreId, 0, 4, GETDATE()),
    (N'Jhikargachha', N'ঝিকরগাছা', @JashoreId, 0, 5, GETDATE()),
    (N'Keshabpur', N'কেশবপুর', @JashoreId, 0, 6, GETDATE()),
    (N'Manirampur', N'মণিরামপুর', @JashoreId, 0, 7, GETDATE()),
    (N'Sharsha', N'শার্শা', @JashoreId, 0, 8, GETDATE());
    PRINT 'Inserted Jashore upazilas';
END

-- Jhenaidah District (6 upazilas)
DECLARE @JhenaidahId INT = (SELECT Id FROM Districts WHERE Name = 'Jhenaidah');
IF @JhenaidahId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Jhenaidah Sadar', N'ঝিনাইদহ সদর', @JhenaidahId, 0, 1, GETDATE()),
    (N'Harinakunda', N'হরিণাকুন্ডু', @JhenaidahId, 0, 2, GETDATE()),
    (N'Kaliganj', N'কালীগঞ্জ', @JhenaidahId, 0, 3, GETDATE()),
    (N'Kotchandpur', N'কোটচাঁদপুর', @JhenaidahId, 0, 4, GETDATE()),
    (N'Maheshpur', N'মহেশপুর', @JhenaidahId, 0, 5, GETDATE()),
    (N'Shailkupa', N'শৈলকুপা', @JhenaidahId, 0, 6, GETDATE());
    PRINT 'Inserted Jhenaidah upazilas';
END

-- Khulna District (9 upazilas)
DECLARE @KhulnaId INT = (SELECT Id FROM Districts WHERE Name = 'Khulna');
IF @KhulnaId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Khulna Sadar', N'খুলনা সদর', @KhulnaId, 0, 1, GETDATE()),
    (N'Batiaghata', N'বটিয়াঘাটা', @KhulnaId, 0, 2, GETDATE()),
    (N'Dacope', N'দাকোপ', @KhulnaId, 0, 3, GETDATE()),
    (N'Dighalia', N'ডিঘলিয়া', @KhulnaId, 0, 4, GETDATE()),
    (N'Dumuria', N'ডুমुरিয়া', @KhulnaId, 0, 5, GETDATE()),
    (N'Koyra', N'কয়রা', @KhulnaId, 0, 6, GETDATE()),
    (N'Paikgachha', N'পাইকগাছা', @KhulnaId, 0, 7, GETDATE()),
    (N'Phultala', N'ফুলতলা', @KhulnaId, 0, 8, GETDATE()),
    (N'Rupsha', N'রূপসা', @KhulnaId, 0, 9, GETDATE());
    PRINT 'Inserted Khulna upazilas';
END

-- Kushtia District (6 upazilas)
DECLARE @KushtiaId INT = (SELECT Id FROM Districts WHERE Name = 'Kushtia');
IF @KushtiaId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Kushtia Sadar', N'কুষ্টিয়া সদর', @KushtiaId, 0, 1, GETDATE()),
    (N'Bheramara', N'ভেড়ামারা', @KushtiaId, 0, 2, GETDATE()),
    (N'Daulatpur', N'দৌলতপুর', @KushtiaId, 0, 3, GETDATE()),
    (N'Khoksa', N'খোকসা', @KushtiaId, 0, 4, GETDATE()),
    (N'Kumarkhali', N'কুমারখালি', @KushtiaId, 0, 5, GETDATE()),
    (N'Mirpur', N'মিরপুর', @KushtiaId, 0, 6, GETDATE());
    PRINT 'Inserted Kushtia upazilas';
END

-- Magura District (4 upazilas)
DECLARE @MaguraId INT = (SELECT Id FROM Districts WHERE Name = 'Magura');
IF @MaguraId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Magura Sadar', N'মাগুরা সদর', @MaguraId, 0, 1, GETDATE()),
    (N'Mohammadpur', N'মোহাম্মদপুর', @MaguraId, 0, 2, GETDATE()),
    (N'Shalikha', N'শালিখা', @MaguraId, 0, 3, GETDATE()),
    (N'Sreepur', N'শ্রীপুর', @MaguraId, 0, 4, GETDATE());
    PRINT 'Inserted Magura upazilas';
END

-- Meherpur District (3 upazilas)
DECLARE @MeherpurId INT = (SELECT Id FROM Districts WHERE Name = 'Meherpur');
IF @MeherpurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Meherpur Sadar', N'মেহেরপুর সদর', @MeherpurId, 0, 1, GETDATE()),
    (N'Gangni', N'গাংনী', @MeherpurId, 0, 2, GETDATE()),
    (N'Mujibnagar', N'মুজিবনগর', @MeherpurId, 0, 3, GETDATE());
    PRINT 'Inserted Meherpur upazilas';
END

-- Narail District (3 upazilas)
DECLARE @NarailId INT = (SELECT Id FROM Districts WHERE Name = 'Narail');
IF @NarailId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Narail Sadar', N'নড়াইল সদর', @NarailId, 0, 1, GETDATE()),
    (N'Kalia', N'কালিয়া', @NarailId, 0, 2, GETDATE()),
    (N'Lohagara', N'লোহাগড়া', @NarailId, 0, 3, GETDATE());
    PRINT 'Inserted Narail upazilas';
END

-- Satkhira District (7 upazilas)
DECLARE @SatkhiraId INT = (SELECT Id FROM Districts WHERE Name = 'Satkhira');
IF @SatkhiraId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Satkhira Sadar', N'সাতক্ষীরা সদর', @SatkhiraId, 0, 1, GETDATE()),
    (N'Assasuni', N'আশাশুনি', @SatkhiraId, 0, 2, GETDATE()),
    (N'Debhata', N'দেভাটা', @SatkhiraId, 0, 3, GETDATE()),
    (N'Kalaroa', N'কলারোয়া', @SatkhiraId, 0, 4, GETDATE()),
    (N'Kaliganj', N'কালীগঞ্জ', @SatkhiraId, 0, 5, GETDATE()),
    (N'Shyamnagar', N'শ্যামনগর', @SatkhiraId, 0, 6, GETDATE()),
    (N'Tala', N'তালা', @SatkhiraId, 0, 7, GETDATE());
    PRINT 'Inserted Satkhira upazilas';
END

-- =======================
-- BARISHAL DIVISION
-- =======================

-- Barguna District (6 upazilas)
DECLARE @BargunaId INT = (SELECT Id FROM Districts WHERE Name = 'Barguna');
IF @BargunaId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Barguna Sadar', N'বরগুনা সদর', @BargunaId, 0, 1, GETDATE()),
    (N'Amtali', N'আমতলী', @BargunaId, 0, 2, GETDATE()),
    (N'Bamna', N'বামনা', @BargunaId, 0, 3, GETDATE()),
    (N'Betagi', N'বেতাগী', @BargunaId, 0, 4, GETDATE()),
    (N'Patharghata', N'পাথরঘাটা', @BargunaId, 0, 5, GETDATE()),
    (N'Taltali', N'তালতলী', @BargunaId, 0, 6, GETDATE());
    PRINT 'Inserted Barguna upazilas';
END

-- Barishal District (10 upazilas)
DECLARE @BarishalId INT = (SELECT Id FROM Districts WHERE Name = 'Barishal');
IF @BarishalId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Barishal Sadar', N'বরিশাল সদর', @BarishalId, 0, 1, GETDATE()),
    (N'Agailjhara', N'আগৈলঝাড়া', @BarishalId, 0, 2, GETDATE()),
    (N'Babuganj', N'বাবুগঞ্জ', @BarishalId, 0, 3, GETDATE()),
    (N'Bakerganj', N'বাকেরগঞ্জ', @BarishalId, 0, 4, GETDATE()),
    (N'Banaripara', N'বানারীপাড়া', @BarishalId, 0, 5, GETDATE()),
    (N'Gaurnadi', N'গৌরনদী', @BarishalId, 0, 6, GETDATE()),
    (N'Hizla', N'হিজলা', @BarishalId, 0, 7, GETDATE()),
    (N'Mehendiganj', N'মেহেন্দিগঞ্জ', @BarishalId, 0, 8, GETDATE()),
    (N'Muladi', N'মুলাদী', @BarishalId, 0, 9, GETDATE()),
    (N'Wazirpur', N'উজিরপুর', @BarishalId, 0, 10, GETDATE());
    PRINT 'Inserted Barishal upazilas';
END

-- Bhola District (7 upazilas)
DECLARE @BholaId INT = (SELECT Id FROM Districts WHERE Name = 'Bhola');
IF @BholaId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Bhola Sadar', N'ভোলা সদর', @BholaId, 0, 1, GETDATE()),
    (N'Burhanuddin', N'বুরহানউদ্দিন', @BholaId, 0, 2, GETDATE()),
    (N'Char Fasson', N'চরফ্যাশন', @BholaId, 0, 3, GETDATE()),
    (N'Daulatkhan', N'দৌলতখান', @BholaId, 0, 4, GETDATE()),
    (N'Lalmohan', N'লালমোহন', @BholaId, 0, 5, GETDATE()),
    (N'Manpura', N'মনপুরা', @BholaId, 0, 6, GETDATE()),
    (N'Tazumuddin', N'তজুমদ্দিন', @BholaId, 0, 7, GETDATE());
    PRINT 'Inserted Bhola upazilas';
END

-- Jhalokati District (4 upazilas)
DECLARE @JhalokatiId INT = (SELECT Id FROM Districts WHERE Name = 'Jhalokati');
IF @JhalokatiId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Jhalokati Sadar', N'ঝালকাঠি সদর', @JhalokatiId, 0, 1, GETDATE()),
    (N'Kathalia', N'কাঠালিয়া', @JhalokatiId, 0, 2, GETDATE()),
    (N'Nalchity', N'নলছিটি', @JhalokatiId, 0, 3, GETDATE()),
    (N'Rajapur', N'রাজাপুর', @JhalokatiId, 0, 4, GETDATE());
    PRINT 'Inserted Jhalokati upazilas';
END

-- Patuakhali District (8 upazilas)
DECLARE @PatuakhaliId INT = (SELECT Id FROM Districts WHERE Name = 'Patuakhali');
IF @PatuakhaliId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Patuakhali Sadar', N'পটুয়াখালী সদর', @PatuakhaliId, 0, 1, GETDATE()),
    (N'Bauphal', N'বাউফল', @PatuakhaliId, 0, 2, GETDATE()),
    (N'Dashmina', N'দশমিনা', @PatuakhaliId, 0, 3, GETDATE()),
    (N'Galachipa', N'গলাচিপা', @PatuakhaliId, 0, 4, GETDATE()),
    (N'Kalapara', N'কলাপাড়া', @PatuakhaliId, 0, 5, GETDATE()),
    (N'Mirzaganj', N'মির্জাগঞ্জ', @PatuakhaliId, 0, 6, GETDATE()),
    (N'Dumki', N'দুমকি', @PatuakhaliId, 0, 7, GETDATE()),
    (N'Rangabali', N'রাঙ্গাবালি', @PatuakhaliId, 0, 8, GETDATE());
    PRINT 'Inserted Patuakhali upazilas';
END

-- Pirojpur District (7 upazilas)
DECLARE @PirojpurId INT = (SELECT Id FROM Districts WHERE Name = 'Pirojpur');
IF @PirojpurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Pirojpur Sadar', N'পিরোজপুর সদর', @PirojpurId, 0, 1, GETDATE()),
    (N'Bhandaria', N'ভান্ডারিয়া', @PirojpurId, 0, 2, GETDATE()),
    (N'Kawkhali', N'কাউখালি', @PirojpurId, 0, 3, GETDATE()),
    (N'Mathbaria', N'মাঠবাড়িয়া', @PirojpurId, 0, 4, GETDATE()),
    (N'Nazirpur', N'নাজিরপুর', @PirojpurId, 0, 5, GETDATE()),
    (N'Nesarabad', N'নেসারাবাদ', @PirojpurId, 0, 6, GETDATE()),
    (N'Indurkani', N'ইন্দুরকানি', @PirojpurId, 0, 7, GETDATE());
    PRINT 'Inserted Pirojpur upazilas';
END

PRINT 'Part 2 completed! Rajshahi, Khulna, and Barishal divisions completed.';
PRINT 'Continue with Part 3 for remaining divisions...';
