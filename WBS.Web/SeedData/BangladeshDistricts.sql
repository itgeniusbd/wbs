-- Sample Bangladesh Districts Data with Coordinates
-- This script adds the main districts of Bangladesh with approximate lat/long coordinates
-- IMPORTANT: Run this script in SQL Server Management Studio or Azure Data Studio with UTF-8 encoding

-- Note: Run this script after running migrations

-- Clear existing data if needed (uncomment if you want to reset)
-- DELETE FROM Upazilas;
-- DELETE FROM Districts;

-- Dhaka Division
INSERT INTO Districts (Name, NameBn, HasWork, Latitude, Longitude, DisplayOrder, CreatedAt) VALUES
(N'Dhaka', N'ঢাকা', 0, 23.8103, 90.4125, 1, GETDATE()),
(N'Faridpur', N'ফরিদপুর', 0, 23.6070, 89.8429, 2, GETDATE()),
(N'Gazipur', N'গাজীপুর', 0, 24.0022, 90.4264, 3, GETDATE()),
(N'Gopalganj', N'গোপালগঞ্জ', 0, 23.0050, 89.8266, 4, GETDATE()),
(N'Kishoreganj', N'কিশোরগঞ্জ', 0, 24.4260, 90.7767, 5, GETDATE()),
(N'Madaripur', N'মাদারীপুর', 0, 23.1641, 90.1897, 6, GETDATE()),
(N'Manikganj', N'মানিকগঞ্জ', 0, 23.8644, 90.0047, 7, GETDATE()),
(N'Munshiganj', N'মুন্সিগঞ্জ', 0, 23.5422, 90.5305, 8, GETDATE()),
(N'Narayanganj', N'নারায়ণগঞ্জ', 0, 23.6238, 90.4968, 9, GETDATE()),
(N'Narsingdi', N'নরসিংদী', 0, 23.9322, 90.7151, 10, GETDATE()),
(N'Rajbari', N'রাজবাড়ী', 0, 23.7574, 89.6444, 11, GETDATE()),
(N'Shariatpur', N'শরীয়তপুর', 0, 23.2423, 90.4348, 12, GETDATE()),
(N'Tangail', N'টাঙ্গাইল', 0, 24.2513, 89.9167, 13, GETDATE());

-- Chittagong Division
INSERT INTO Districts (Name, NameBn, HasWork, Latitude, Longitude, DisplayOrder, CreatedAt) VALUES
(N'Bandarban', N'বান্দরবান', 0, 22.1953, 92.2183, 14, GETDATE()),
(N'Brahmanbaria', N'ব্রাহ্মণবাড়িয়া', 0, 23.9571, 91.1115, 15, GETDATE()),
(N'Chandpur', N'চাঁদপুর', 0, 23.2332, 90.6712, 16, GETDATE()),
(N'Chattogram', N'চট্টগ্রাম', 0, 22.3569, 91.7832, 17, GETDATE()),
(N'Cumilla', N'কুমিল্লা', 0, 23.4607, 91.1809, 18, GETDATE()),
(N'Cox''s Bazar', N'কক্সবাজার', 0, 21.4272, 92.0058, 19, GETDATE()),
(N'Feni', N'ফেনী', 0, 23.0159, 91.3976, 20, GETDATE()),
(N'Khagrachari', N'খাগড়াছড়ি', 0, 23.1193, 91.9847, 21, GETDATE()),
(N'Lakshmipur', N'লক্ষ্মীপুর', 0, 22.9447, 90.8298, 22, GETDATE()),
(N'Noakhali', N'নোয়াখালী', 0, 22.8696, 91.0995, 23, GETDATE()),
(N'Rangamati', N'রাঙ্গামাটি', 0, 22.7324, 92.2985, 24, GETDATE());

-- Rajshahi Division
INSERT INTO Districts (Name, NameBn, HasWork, Latitude, Longitude, DisplayOrder, CreatedAt) VALUES
(N'Bogura', N'বগুড়া', 0, 24.8465, 89.3770, 25, GETDATE()),
(N'Joypurhat', N'জয়পুরহাট', 0, 25.0968, 89.0227, 26, GETDATE()),
(N'Naogaon', N'নওগাঁ', 0, 24.7936, 88.9318, 27, GETDATE()),
(N'Natore', N'নাটোর', 0, 24.4206, 89.0000, 28, GETDATE()),
(N'Chapainawabganj', N'চাঁপাইনবাবগঞ্জ', 0, 24.5965, 88.2775, 29, GETDATE()),
(N'Pabna', N'পাবনা', 0, 24.0064, 89.2372, 30, GETDATE()),
(N'Rajshahi', N'রাজশাহী', 0, 24.3745, 88.6042, 31, GETDATE()),
(N'Sirajganj', N'সিরাজগঞ্জ', 0, 24.4533, 89.7006, 32, GETDATE());

-- Khulna Division
INSERT INTO Districts (Name, NameBn, HasWork, Latitude, Longitude, DisplayOrder, CreatedAt) VALUES
(N'Bagerhat', N'বাগেরহাট', 0, 22.6602, 89.7895, 33, GETDATE()),
(N'Chuadanga', N'চুয়াডাঙ্গা', 0, 23.6401, 88.8412, 34, GETDATE()),
(N'Jashore', N'যশোর', 0, 23.1634, 89.2182, 35, GETDATE()),
(N'Jhenaidah', N'ঝিনাইদহ', 0, 23.5448, 89.1539, 36, GETDATE()),
(N'Khulna', N'খুলনা', 0, 22.8456, 89.5403, 37, GETDATE()),
(N'Kushtia', N'কুষ্টিয়া', 0, 23.9013, 89.1205, 38, GETDATE()),
(N'Magura', N'মাগুরা', 0, 23.4855, 89.4198, 39, GETDATE()),
(N'Meherpur', N'মেহেরপুর', 0, 23.7622, 88.6318, 40, GETDATE()),
(N'Narail', N'নড়াইল', 0, 23.1163, 89.5840, 41, GETDATE()),
(N'Satkhira', N'সাতক্ষীরা', 0, 22.7185, 89.0705, 42, GETDATE());

-- Barishal Division
INSERT INTO Districts (Name, NameBn, HasWork, Latitude, Longitude, DisplayOrder, CreatedAt) VALUES
(N'Barguna', N'বরগুনা', 0, 22.1595, 90.1119, 43, GETDATE()),
(N'Barishal', N'বরিশাল', 0, 22.7010, 90.3535, 44, GETDATE()),
(N'Bhola', N'ভোলা', 0, 22.6859, 90.6482, 45, GETDATE()),
(N'Jhalokati', N'ঝালকাঠি', 0, 22.6406, 90.1987, 46, GETDATE()),
(N'Patuakhali', N'পটুয়াখালী', 0, 22.3596, 90.3298, 47, GETDATE()),
(N'Pirojpur', N'পিরোজপুর', 0, 22.5841, 89.9720, 48, GETDATE());

-- Sylhet Division
INSERT INTO Districts (Name, NameBn, HasWork, Latitude, Longitude, DisplayOrder, CreatedAt) VALUES
(N'Habiganj', N'হবিগঞ্জ', 0, 24.3745, 91.4152, 49, GETDATE()),
(N'Moulvibazar', N'মৌলভীবাজার', 0, 24.4829, 91.7774, 50, GETDATE()),
(N'Sunamganj', N'সুনামগঞ্জ', 0, 25.0658, 91.3950, 51, GETDATE()),
(N'Sylhet', N'সিলেট', 0, 24.8949, 91.8687, 52, GETDATE());

-- Rangpur Division
INSERT INTO Districts (Name, NameBn, HasWork, Latitude, Longitude, DisplayOrder, CreatedAt) VALUES
(N'Dinajpur', N'দিনাজপুর', 0, 25.6279, 88.6332, 53, GETDATE()),
(N'Gaibandha', N'গাইবান্ধা', 0, 25.3288, 89.5281, 54, GETDATE()),
(N'Kurigram', N'কুড়িগ্রাম', 0, 25.8073, 89.6361, 55, GETDATE()),
(N'Lalmonirhat', N'লালমনিরহাট', 0, 25.9923, 89.2847, 56, GETDATE()),
(N'Nilphamari', N'নীলফামারী', 0, 25.9317, 88.8560, 57, GETDATE()),
(N'Panchagarh', N'পঞ্চগড়', 0, 26.3411, 88.5541, 58, GETDATE()),
(N'Rangpur', N'রংপুর', 0, 25.7439, 89.2752, 59, GETDATE()),
(N'Thakurgaon', N'ঠাকুরগাঁও', 0, 26.0336, 88.4616, 60, GETDATE());

-- Mymensingh Division
INSERT INTO Districts (Name, NameBn, HasWork, Latitude, Longitude, DisplayOrder, CreatedAt) VALUES
(N'Jamalpur', N'জামালপুর', 0, 25.0831, 89.9378, 61, GETDATE()),
(N'Mymensingh', N'ময়মনসিংহ', 0, 24.7471, 90.4203, 62, GETDATE()),
(N'Netrokona', N'নেত্রকোণা', 0, 24.8104, 90.7278, 63, GETDATE()),
(N'Sherpur', N'শেরপুর', 0, 25.0204, 90.0152, 64, GETDATE());

PRINT 'Successfully inserted 64 districts of Bangladesh with Bengali names';
PRINT 'Note: The N prefix before strings ensures Unicode (NVARCHAR) storage for Bengali characters';
