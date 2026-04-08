-- Complete Bangladesh Upazilas Data
-- This script adds all upazilas of Bangladesh with Bengali names
-- IMPORTANT: Run this AFTER running the Districts insert/update scripts
-- Run this in SQL Server Management Studio or Azure Data Studio with UTF-8 encoding

-- Note: HasWork = 0 means WBS doesn't work there yet (default)
--       HasWork = 1 means WBS is working there
--       You can update these values later from Admin Panel

PRINT 'Starting to insert Upazilas...';

-- =======================
-- DHAKA DIVISION
-- =======================

-- Dhaka District (23 upazilas)
DECLARE @DhakaId INT = (SELECT Id FROM Districts WHERE Name = 'Dhaka');
IF @DhakaId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Dhamrai', N'??????', @DhakaId, 0, 1, GETDATE()),
    (N'Dohar', N'?????', @DhakaId, 0, 2, GETDATE()),
    (N'Keraniganj', N'??????????', @DhakaId, 0, 3, GETDATE()),
    (N'Nawabganj', N'????????', @DhakaId, 0, 4, GETDATE()),
    (N'Savar', N'?????', @DhakaId, 0, 5, GETDATE()),
    (N'Tejgaon', N'???????', @DhakaId, 0, 6, GETDATE()),
    (N'Mohammadpur', N'???????????', @DhakaId, 0, 7, GETDATE()),
    (N'Dhanmondi', N'????????', @DhakaId, 0, 8, GETDATE()),
    (N'Ramna', N'????', @DhakaId, 0, 9, GETDATE()),
    (N'Motijheel', N'??????', @DhakaId, 0, 10, GETDATE()),
    (N'Sabujbagh', N'???????', @DhakaId, 0, 11, GETDATE()),
    (N'Demra', N'?????', @DhakaId, 0, 12, GETDATE()),
    (N'Kotwali', N'?????????', @DhakaId, 0, 13, GETDATE()),
    (N'Sutrapur', N'?????????', @DhakaId, 0, 14, GETDATE()),
    (N'Lalbagh', N'??????', @DhakaId, 0, 15, GETDATE()),
    (N'Kamrangirchar', N'????????????', @DhakaId, 0, 16, GETDATE()),
    (N'Hazaribagh', N'?????????', @DhakaId, 0, 17, GETDATE()),
    (N'Gulshan', N'??????', @DhakaId, 0, 18, GETDATE()),
    (N'Mirpur', N'??????', @DhakaId, 0, 19, GETDATE()),
    (N'Pallabi', N'??????', @DhakaId, 0, 20, GETDATE()),
    (N'Cantonment', N'?????????????', @DhakaId, 0, 21, GETDATE()),
    (N'Uttara', N'??????', @DhakaId, 0, 22, GETDATE()),
    (N'Dakshinkhan', N'?????????', @DhakaId, 0, 23, GETDATE());
    PRINT 'Inserted Dhaka upazilas';
END

-- Faridpur District (9 upazilas)
DECLARE @FaridpurId INT = (SELECT Id FROM Districts WHERE Name = 'Faridpur');
IF @FaridpurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Faridpur Sadar', N'??????? ???', @FaridpurId, 0, 1, GETDATE()),
    (N'Alfadanga', N'??????????', @FaridpurId, 0, 2, GETDATE()),
    (N'Boalmari', N'??????????', @FaridpurId, 0, 3, GETDATE()),
    (N'Char Bhadrasan', N'?????????', @FaridpurId, 0, 4, GETDATE()),
    (N'Madhukhali', N'???????', @FaridpurId, 0, 5, GETDATE()),
    (N'Nagarkanda', N'?????????', @FaridpurId, 0, 6, GETDATE()),
    (N'Sadarpur', N'??????', @FaridpurId, 0, 7, GETDATE()),
    (N'Saltha', N'?????', @FaridpurId, 0, 8, GETDATE()),
    (N'Bhanga', N'??????', @FaridpurId, 0, 9, GETDATE());
    PRINT 'Inserted Faridpur upazilas';
END

-- Gazipur District (5 upazilas)
DECLARE @GazipurId INT = (SELECT Id FROM Districts WHERE Name = 'Gazipur');
IF @GazipurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Gazipur Sadar', N'??????? ???', @GazipurId, 0, 1, GETDATE()),
    (N'Kaliakair', N'??????????', @GazipurId, 0, 2, GETDATE()),
    (N'Kapasia', N'?????????', @GazipurId, 0, 3, GETDATE()),
    (N'Sreepur', N'???????', @GazipurId, 0, 4, GETDATE()),
    (N'Kaliganj', N'????????', @GazipurId, 0, 5, GETDATE());
    PRINT 'Inserted Gazipur upazilas';
END

-- Gopalganj District (5 upazilas)
DECLARE @GopalganjId INT = (SELECT Id FROM Districts WHERE Name = 'Gopalganj');
IF @GopalganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Gopalganj Sadar', N'????????? ???', @GopalganjId, 0, 1, GETDATE()),
    (N'Kashiani', N'?????????', @GopalganjId, 0, 2, GETDATE()),
    (N'Kotalipara', N'???????????', @GopalganjId, 0, 3, GETDATE()),
    (N'Muksudpur', N'?????????', @GopalganjId, 0, 4, GETDATE()),
    (N'Tungipara', N'???????????', @GopalganjId, 0, 5, GETDATE());
    PRINT 'Inserted Gopalganj upazilas';
END

-- Kishoreganj District (13 upazilas)
DECLARE @KishoreganjId INT = (SELECT Id FROM Districts WHERE Name = 'Kishoreganj');
IF @KishoreganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Kishoreganj Sadar', N'????????? ???', @KishoreganjId, 0, 1, GETDATE()),
    (N'Austagram', N'?????????', @KishoreganjId, 0, 2, GETDATE()),
    (N'Bajitpur', N'????????', @KishoreganjId, 0, 3, GETDATE()),
    (N'Bhairab', N'????', @KishoreganjId, 0, 4, GETDATE()),
    (N'Hossainpur', N'????????', @KishoreganjId, 0, 5, GETDATE()),
    (N'Itna', N'????', @KishoreganjId, 0, 6, GETDATE()),
    (N'Karimganj', N'????????', @KishoreganjId, 0, 7, GETDATE()),
    (N'Katiadi', N'????????', @KishoreganjId, 0, 8, GETDATE()),
    (N'Kuliarchar', N'??????????', @KishoreganjId, 0, 9, GETDATE()),
    (N'Mithamain', N'???????', @KishoreganjId, 0, 10, GETDATE()),
    (N'Nikli', N'?????', @KishoreganjId, 0, 11, GETDATE()),
    (N'Pakundia', N'???????????', @KishoreganjId, 0, 12, GETDATE()),
    (N'Tarail', N'???????', @KishoreganjId, 0, 13, GETDATE());
    PRINT 'Inserted Kishoreganj upazilas';
END

-- Madaripur District (4 upazilas)
DECLARE @MadaripurId INT = (SELECT Id FROM Districts WHERE Name = 'Madaripur');
IF @MadaripurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Madaripur Sadar', N'????????? ???', @MadaripurId, 0, 1, GETDATE()),
    (N'Kalkini', N'???????', @MadaripurId, 0, 2, GETDATE()),
    (N'Rajoir', N'?????', @MadaripurId, 0, 3, GETDATE()),
    (N'Shibchar', N'?????', @MadaripurId, 0, 4, GETDATE());
    PRINT 'Inserted Madaripur upazilas';
END

-- Manikganj District (7 upazilas)
DECLARE @ManikganjId INT = (SELECT Id FROM Districts WHERE Name = 'Manikganj');
IF @ManikganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Manikganj Sadar', N'????????? ???', @ManikganjId, 0, 1, GETDATE()),
    (N'Daulatpur', N'???????', @ManikganjId, 0, 2, GETDATE()),
    (N'Ghior', N'????', @ManikganjId, 0, 3, GETDATE()),
    (N'Harirampur', N'?????????', @ManikganjId, 0, 4, GETDATE()),
    (N'Saturia', N'?????????', @ManikganjId, 0, 5, GETDATE()),
    (N'Shivalaya', N'???????', @ManikganjId, 0, 6, GETDATE()),
    (N'Singair', N'???????', @ManikganjId, 0, 7, GETDATE());
    PRINT 'Inserted Manikganj upazilas';
END

-- Munshiganj District (6 upazilas)
DECLARE @MunshiganjId INT = (SELECT Id FROM Districts WHERE Name = 'Munshiganj');
IF @MunshiganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Munshiganj Sadar', N'?????????? ???', @MunshiganjId, 0, 1, GETDATE()),
    (N'Gazaria', N'????????', @MunshiganjId, 0, 2, GETDATE()),
    (N'Lohajang', N'??????', @MunshiganjId, 0, 3, GETDATE()),
    (N'Sirajdikhan', N'??????????', @MunshiganjId, 0, 4, GETDATE()),
    (N'Sreenagar', N'???????', @MunshiganjId, 0, 5, GETDATE()),
    (N'Tongibari', N'?????????', @MunshiganjId, 0, 6, GETDATE());
    PRINT 'Inserted Munshiganj upazilas';
END

-- Narayanganj District (5 upazilas)
DECLARE @NarayanganjId INT = (SELECT Id FROM Districts WHERE Name = 'Narayanganj');
IF @NarayanganjId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Narayanganj Sadar', N'??????????? ???', @NarayanganjId, 0, 1, GETDATE()),
    (N'Araihazar', N'??????????', @NarayanganjId, 0, 2, GETDATE()),
    (N'Bandar', N'?????', @NarayanganjId, 0, 3, GETDATE()),
    (N'Rupganj', N'???????', @NarayanganjId, 0, 4, GETDATE()),
    (N'Sonargaon', N'?????????', @NarayanganjId, 0, 5, GETDATE());
    PRINT 'Inserted Narayanganj upazilas';
END

-- Narsingdi District (6 upazilas)
DECLARE @NarsingdiId INT = (SELECT Id FROM Districts WHERE Name = 'Narsingdi');
IF @NarsingdiId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Narsingdi Sadar', N'??????? ???', @NarsingdiId, 0, 1, GETDATE()),
    (N'Belabo', N'??????', @NarsingdiId, 0, 2, GETDATE()),
    (N'Monohardi', N'???????', @NarsingdiId, 0, 3, GETDATE()),
    (N'Palash', N'????', @NarsingdiId, 0, 4, GETDATE()),
    (N'Raipura', N'????????', @NarsingdiId, 0, 5, GETDATE()),
    (N'Shibpur', N'??????', @NarsingdiId, 0, 6, GETDATE());
    PRINT 'Inserted Narsingdi upazilas';
END

-- Rajbari District (5 upazilas)
DECLARE @RajbariId INT = (SELECT Id FROM Districts WHERE Name = 'Rajbari');
IF @RajbariId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Rajbari Sadar', N'???????? ???', @RajbariId, 0, 1, GETDATE()),
    (N'Baliakandi', N'?????????????', @RajbariId, 0, 2, GETDATE()),
    (N'Goalandaghat', N'????????????', @RajbariId, 0, 3, GETDATE()),
    (N'Kalukhali', N'????????', @RajbariId, 0, 4, GETDATE()),
    (N'Pangsha', N'?????', @RajbariId, 0, 5, GETDATE());
    PRINT 'Inserted Rajbari upazilas';
END

-- Shariatpur District (7 upazilas)
DECLARE @ShariatpurId INT = (SELECT Id FROM Districts WHERE Name = 'Shariatpur');
IF @ShariatpurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Shariatpur Sadar', N'????????? ???', @ShariatpurId, 0, 1, GETDATE()),
    (N'Bhedarganj', N'????????', @ShariatpurId, 0, 2, GETDATE()),
    (N'Damudya', N'????????', @ShariatpurId, 0, 3, GETDATE()),
    (N'Gosairhat', N'?????????', @ShariatpurId, 0, 4, GETDATE()),
    (N'Naria', N'???????', @ShariatpurId, 0, 5, GETDATE()),
    (N'Zajira', N'??????', @ShariatpurId, 0, 6, GETDATE()),
    (N'Shakhipur', N'??????', @ShariatpurId, 0, 7, GETDATE());
    PRINT 'Inserted Shariatpur upazilas';
END

-- Tangail District (12 upazilas)
DECLARE @TangailId INT = (SELECT Id FROM Districts WHERE Name = 'Tangail');
IF @TangailId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Tangail Sadar', N'???????? ???', @TangailId, 0, 1, GETDATE()),
    (N'Basail', N'??????', @TangailId, 0, 2, GETDATE()),
    (N'Bhuapur', N'????????', @TangailId, 0, 3, GETDATE()),
    (N'Delduar', N'?????????', @TangailId, 0, 4, GETDATE()),
    (N'Dhanbari', N'???????', @TangailId, 0, 5, GETDATE()),
    (N'Ghatail', N'??????', @TangailId, 0, 6, GETDATE()),
    (N'Gopalpur', N'????????', @TangailId, 0, 7, GETDATE()),
    (N'Kalihati', N'????????', @TangailId, 0, 8, GETDATE()),
    (N'Madhupur', N'??????', @TangailId, 0, 9, GETDATE()),
    (N'Mirzapur', N'?????????', @TangailId, 0, 10, GETDATE()),
    (N'Nagarpur', N'???????', @TangailId, 0, 11, GETDATE()),
    (N'Sakhipur', N'??????', @TangailId, 0, 12, GETDATE());
    PRINT 'Inserted Tangail upazilas';
END

-- =======================
-- CHITTAGONG DIVISION
-- =======================

-- Bandarban District (7 upazilas)
DECLARE @BandarbanId INT = (SELECT Id FROM Districts WHERE Name = 'Bandarban');
IF @BandarbanId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Bandarban Sadar', N'????????? ???', @BandarbanId, 0, 1, GETDATE()),
    (N'Alikadam', N'??????', @BandarbanId, 0, 2, GETDATE()),
    (N'Lama', N'????', @BandarbanId, 0, 3, GETDATE()),
    (N'Naikhongchhari', N'?????????????', @BandarbanId, 0, 4, GETDATE()),
    (N'Rowangchhari', N'??????????', @BandarbanId, 0, 5, GETDATE()),
    (N'Ruma', N'????', @BandarbanId, 0, 6, GETDATE()),
    (N'Thanchi', N'?????', @BandarbanId, 0, 7, GETDATE());
    PRINT 'Inserted Bandarban upazilas';
END

-- Brahmanbaria District (9 upazilas)
DECLARE @BrahmanbariaId INT = (SELECT Id FROM Districts WHERE Name = 'Brahmanbaria');
IF @BrahmanbariaId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Brahmanbaria Sadar', N'???????????????? ???', @BrahmanbariaId, 0, 1, GETDATE()),
    (N'Ashuganj', N'???????', @BrahmanbariaId, 0, 2, GETDATE()),
    (N'Akhaura', N'???????', @BrahmanbariaId, 0, 3, GETDATE()),
    (N'Bancharampur', N'????????????', @BrahmanbariaId, 0, 4, GETDATE()),
    (N'Bijoynagar', N'????????', @BrahmanbariaId, 0, 5, GETDATE()),
    (N'Kasba', N'????', @BrahmanbariaId, 0, 6, GETDATE()),
    (N'Nabinagar', N'??????', @BrahmanbariaId, 0, 7, GETDATE()),
    (N'Nasirnagar', N'????????', @BrahmanbariaId, 0, 8, GETDATE()),
    (N'Sarail', N'?????', @BrahmanbariaId, 0, 9, GETDATE());
    PRINT 'Inserted Brahmanbaria upazilas';
END

-- Chandpur District (8 upazilas)
DECLARE @ChandpurId INT = (SELECT Id FROM Districts WHERE Name = 'Chandpur');
IF @ChandpurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Chandpur Sadar', N'??????? ???', @ChandpurId, 0, 1, GETDATE()),
    (N'Faridganj', N'????????', @ChandpurId, 0, 2, GETDATE()),
    (N'Haimchar', N'??????', @ChandpurId, 0, 3, GETDATE()),
    (N'Haziganj', N'????????', @ChandpurId, 0, 4, GETDATE()),
    (N'Kachua', N'??????', @ChandpurId, 0, 5, GETDATE()),
    (N'Matlab Dakshin', N'???? ??????', @ChandpurId, 0, 6, GETDATE()),
    (N'Matlab Uttar', N'???? ?????', @ChandpurId, 0, 7, GETDATE()),
    (N'Shahrasti', N'?????????', @ChandpurId, 0, 8, GETDATE());
    PRINT 'Inserted Chandpur upazilas';
END

-- Chattogram District (15 upazilas)
DECLARE @ChattogramId INT = (SELECT Id FROM Districts WHERE Name = 'Chattogram');
IF @ChattogramId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Anwara', N'????????', @ChattogramId, 0, 1, GETDATE()),
    (N'Banshkhali', N'????????', @ChattogramId, 0, 2, GETDATE()),
    (N'Boalkhali', N'??????????', @ChattogramId, 0, 3, GETDATE()),
    (N'Chandanaish', N'????????', @ChattogramId, 0, 4, GETDATE()),
    (N'Chattogram Sadar', N'????????? ???', @ChattogramId, 0, 5, GETDATE()),
    (N'Fatikchhari', N'????????', @ChattogramId, 0, 6, GETDATE()),
    (N'Hathazari', N'?????????', @ChattogramId, 0, 7, GETDATE()),
    (N'Lohagara', N'?????????', @ChattogramId, 0, 8, GETDATE()),
    (N'Mirsharai', N'???????', @ChattogramId, 0, 9, GETDATE()),
    (N'Patiya', N'??????', @ChattogramId, 0, 10, GETDATE()),
    (N'Rangunia', N'???????????', @ChattogramId, 0, 11, GETDATE()),
    (N'Raozan', N'??????', @ChattogramId, 0, 12, GETDATE()),
    (N'Sandwip', N'????????', @ChattogramId, 0, 13, GETDATE()),
    (N'Satkania', N'??????????', @ChattogramId, 0, 14, GETDATE()),
    (N'Sitakunda', N'?????????', @ChattogramId, 0, 15, GETDATE());
    PRINT 'Inserted Chattogram upazilas';
END

-- Cumilla District (17 upazilas)
DECLARE @CumillaId INT = (SELECT Id FROM Districts WHERE Name = 'Cumilla');
IF @CumillaId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Cumilla Sadar', N'???????? ???', @CumillaId, 0, 1, GETDATE()),
    (N'Barura', N'??????', @CumillaId, 0, 2, GETDATE()),
    (N'Brahmanpara', N'?????????????', @CumillaId, 0, 3, GETDATE()),
    (N'Burichang', N'???????', @CumillaId, 0, 4, GETDATE()),
    (N'Chandina', N'????????', @CumillaId, 0, 5, GETDATE()),
    (N'Chauddagram', N'??????????', @CumillaId, 0, 6, GETDATE()),
    (N'Daudkandi', N'??????????', @CumillaId, 0, 7, GETDATE()),
    (N'Debidwar', N'?????????', @CumillaId, 0, 8, GETDATE()),
    (N'Homna', N'?????', @CumillaId, 0, 9, GETDATE()),
    (N'Laksam', N'??????', @CumillaId, 0, 10, GETDATE()),
    (N'Langalkot', N'?????????', @CumillaId, 0, 11, GETDATE()),
    (N'Meghna', N'?????', @CumillaId, 0, 12, GETDATE()),
    (N'Monohargonj', N'?????????', @CumillaId, 0, 13, GETDATE()),
    (N'Muradnagar', N'????????', @CumillaId, 0, 14, GETDATE()),
    (N'Nangalkot', N'?????????', @CumillaId, 0, 15, GETDATE()),
    (N'Titas', N'?????', @CumillaId, 0, 16, GETDATE()),
    (N'Cumilla Sadar Dakshin', N'???????? ??? ??????', @CumillaId, 0, 17, GETDATE());
    PRINT 'Inserted Cumilla upazilas';
END

-- Cox's Bazar District (8 upazilas)
DECLARE @CoxsBazarId INT = (SELECT Id FROM Districts WHERE Name = 'Cox''s Bazar');
IF @CoxsBazarId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Cox''s Bazar Sadar', N'????????? ???', @CoxsBazarId, 0, 1, GETDATE()),
    (N'Chakaria', N'???????', @CoxsBazarId, 0, 2, GETDATE()),
    (N'Kutubdia', N'??????????', @CoxsBazarId, 0, 3, GETDATE()),
    (N'Maheshkhali', N'????????', @CoxsBazarId, 0, 4, GETDATE()),
    (N'Pekua', N'???????', @CoxsBazarId, 0, 5, GETDATE()),
    (N'Ramu', N'????', @CoxsBazarId, 0, 6, GETDATE()),
    (N'Teknaf', N'??????', @CoxsBazarId, 0, 7, GETDATE()),
    (N'Ukhia', N'??????', @CoxsBazarId, 0, 8, GETDATE());
    PRINT 'Inserted Cox''s Bazar upazilas';
END

-- Feni District (6 upazilas)
DECLARE @FeniId INT = (SELECT Id FROM Districts WHERE Name = 'Feni');
IF @FeniId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Feni Sadar', N'???? ???', @FeniId, 0, 1, GETDATE()),
    (N'Chhagalnaiya', N'??????????', @FeniId, 0, 2, GETDATE()),
    (N'Daganbhuiyan', N'????????', @FeniId, 0, 3, GETDATE()),
    (N'Fulgazi', N'???????', @FeniId, 0, 4, GETDATE()),
    (N'Parshuram', N'???????', @FeniId, 0, 5, GETDATE()),
    (N'Sonagazi', N'????????', @FeniId, 0, 6, GETDATE());
    PRINT 'Inserted Feni upazilas';
END

-- Khagrachari District (9 upazilas)
DECLARE @KhagrachariId INT = (SELECT Id FROM Districts WHERE Name = 'Khagrachari');
IF @KhagrachariId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Khagrachari Sadar', N'?????????? ???', @KhagrachariId, 0, 1, GETDATE()),
    (N'Dighinala', N'????????', @KhagrachariId, 0, 2, GETDATE()),
    (N'Lakshmichhari', N'?????????', @KhagrachariId, 0, 3, GETDATE()),
    (N'Mahalchhari', N'????????', @KhagrachariId, 0, 4, GETDATE()),
    (N'Manikchhari', N'?????????', @KhagrachariId, 0, 5, GETDATE()),
    (N'Matiranga', N'??????????', @KhagrachariId, 0, 6, GETDATE()),
    (N'Panchhari', N'???????', @KhagrachariId, 0, 7, GETDATE()),
    (N'Ramgarh', N'??????', @KhagrachariId, 0, 8, GETDATE()),
    (N'Guimara', N'???????', @KhagrachariId, 0, 9, GETDATE());
    PRINT 'Inserted Khagrachari upazilas';
END

-- Lakshmipur District (5 upazilas)
DECLARE @LakshmipurId INT = (SELECT Id FROM Districts WHERE Name = 'Lakshmipur');
IF @LakshmipurId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Lakshmipur Sadar', N'?????????? ???', @LakshmipurId, 0, 1, GETDATE()),
    (N'Kamalnagar', N'??????', @LakshmipurId, 0, 2, GETDATE()),
    (N'Raipur', N'???????', @LakshmipurId, 0, 3, GETDATE()),
    (N'Ramganj', N'???????', @LakshmipurId, 0, 4, GETDATE()),
    (N'Ramgati', N'??????', @LakshmipurId, 0, 5, GETDATE());
    PRINT 'Inserted Lakshmipur upazilas';
END

-- Noakhali District (9 upazilas)
DECLARE @NoakhaliId INT = (SELECT Id FROM Districts WHERE Name = 'Noakhali');
IF @NoakhaliId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Noakhali Sadar', N'????????? ???', @NoakhaliId, 0, 1, GETDATE()),
    (N'Begumganj', N'????????', @NoakhaliId, 0, 2, GETDATE()),
    (N'Chatkhil', N'??????', @NoakhaliId, 0, 3, GETDATE()),
    (N'Companiganj', N'????????????', @NoakhaliId, 0, 4, GETDATE()),
    (N'Hatiya', N'???????', @NoakhaliId, 0, 5, GETDATE()),
    (N'Kabirhat', N'???????', @NoakhaliId, 0, 6, GETDATE()),
    (N'Senbagh', N'??????', @NoakhaliId, 0, 7, GETDATE()),
    (N'Sonaimuri', N'??????????', @NoakhaliId, 0, 8, GETDATE()),
    (N'Subarnachar', N'????????', @NoakhaliId, 0, 9, GETDATE());
    PRINT 'Inserted Noakhali upazilas';
END

-- Rangamati District (10 upazilas)
DECLARE @RangamatiId INT = (SELECT Id FROM Districts WHERE Name = 'Rangamati');
IF @RangamatiId IS NOT NULL
BEGIN
    INSERT INTO Upazilas (Name, NameBn, DistrictId, HasWork, DisplayOrder, CreatedAt) VALUES
    (N'Rangamati Sadar', N'?????????? ???', @RangamatiId, 0, 1, GETDATE()),
    (N'Baghaichhari', N'?????????', @RangamatiId, 0, 2, GETDATE()),
    (N'Barkal', N'????', @RangamatiId, 0, 3, GETDATE()),
    (N'Belaichhari', N'?????????', @RangamatiId, 0, 4, GETDATE()),
    (N'Juraichhari', N'????????', @RangamatiId, 0, 5, GETDATE()),
    (N'Kaptai', N'???????', @RangamatiId, 0, 6, GETDATE()),
    (N'Kawkhali', N'???????', @RangamatiId, 0, 7, GETDATE()),
    (N'Langadu', N'????????', @RangamatiId, 0, 8, GETDATE()),
    (N'Naniarchar', N'??????????', @RangamatiId, 0, 9, GETDATE()),
    (N'Rajasthali', N'????????', @RangamatiId, 0, 10, GETDATE());
    PRINT 'Inserted Rangamati upazilas';
END

PRINT 'Chittagong Division upazilas completed!';
PRINT '';
PRINT 'Note: Script will continue with remaining divisions...';
PRINT 'Total upazilas inserted so far: Approximately 200+';
PRINT 'This is Part 1 of 2. Run Part 2 script for remaining divisions.';
