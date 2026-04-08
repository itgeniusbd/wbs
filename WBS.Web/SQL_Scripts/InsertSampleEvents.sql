-- Insert Sample Events/Projects for Testing Statistics
-- Run this script to populate your database with sample data

USE [WbsDb];
GO

-- Insert sample events for "Training center construction" (Program ID = 1)
INSERT INTO SDGProjects (Title, TitleBn, Description, District, Thana, [Union], Village, BeneficiaryCount, SDGId, SDGProgramId, IsActive, IsFeatured, DisplayOrder, CreatedAt)
VALUES 
('Training Center at Dhaka', N'?????? ????????? ???????', 'Training center construction in Dhaka', 'Dhaka', 'Mirpur', 'Mirpur-1', 'Block-C', 150, 1, 1, 1, 1, 1, GETDATE()),
('Training Center at Chittagong', N'?????????? ????????? ???????', 'Training center construction in Chittagong', 'Chittagong', 'Patenga', 'North Patenga', 'Patenga', 200, 1, 1, 1, 0, 2, GETDATE()),
('Training Center at Sylhet', N'?????? ????????? ???????', 'Training center construction in Sylhet', 'Sylhet', 'Dakshin Surma', 'Mogolgaon', 'Mogolgaon', 120, 1, 1, 1, 0, 3, GETDATE());

-- Insert sample events for "Sewing machine distribution" (Program ID = 2)
INSERT INTO SDGProjects (Title, TitleBn, Description, District, Thana, [Union], Village, BeneficiaryCount, SDGId, SDGProgramId, IsActive, IsFeatured, DisplayOrder, CreatedAt)
VALUES 
('Sewing Machine Distribution - Dhaka', N'????? ????? ????? - ????', 'Distributed 50 sewing machines to women', 'Dhaka', 'Uttara', 'Uttara West', 'Sector-7', 50, 1, 2, 1, 1, 1, GETDATE()),
('Sewing Machine Distribution - Gazipur', N'????? ????? ????? - ???????', 'Distributed 75 sewing machines to women', 'Gazipur', 'Gazipur Sadar', 'Tongi', 'Tongi', 75, 1, 2, 1, 0, 2, GETDATE()),
('Sewing Machine Distribution - Narayanganj', N'????? ????? ????? - ???????????', 'Distributed 60 sewing machines to women', 'Narayanganj', 'Narayanganj Sadar', 'Fatullah', 'Fatullah', 60, 1, 2, 1, 0, 3, GETDATE());

-- Insert sample events for "EDUCATION: THE POWER OF KNOWLEDGE" (Program ID = 3)
INSERT INTO SDGProjects (Title, TitleBn, Description, District, Thana, [Union], Village, BeneficiaryCount, SDGId, SDGProgramId, IsActive, IsFeatured, DisplayOrder, CreatedAt)
VALUES 
('Educational Support - Dhaka', N'?????? ??????? - ????', 'Educational support for underprivileged children', 'Dhaka', 'Mohammadpur', 'Mohammadpur', 'Block-B', 300, 4, 3, 1, 1, 1, GETDATE()),
('Educational Support - Comilla', N'?????? ??????? - ????????', 'Educational support for underprivileged children', 'Comilla', 'Comilla Sadar', 'Comilla Sadar', 'Town', 250, 4, 3, 1, 0, 2, GETDATE()),
('Educational Support - Rajshahi', N'?????? ??????? - ???????', 'Educational support for underprivileged children', 'Rajshahi', 'Rajshahi Sadar', 'Rajpara', 'Rajpara', 180, 4, 3, 1, 0, 3, GETDATE()),
('Educational Support - Khulna', N'?????? ??????? - ?????', 'Educational support for underprivileged children', 'Khulna', 'Khulna Sadar', 'Sonadanga', 'Sonadanga', 220, 4, 3, 1, 0, 4, GETDATE());

-- Insert sample events for "Water pump installation" (Program ID = 4)
INSERT INTO SDGProjects (Title, TitleBn, Description, District, Thana, [Union], Village, BeneficiaryCount, SDGId, SDGProgramId, IsActive, IsFeatured, DisplayOrder, CreatedAt)
VALUES 
('Water Pump Installation - Barisal', N'????? ????? ?????? - ??????', 'Installed 10 water pumps in rural areas', 'Barisal', 'Barisal Sadar', 'Chandmari', 'Chandmari', 500, 6, 4, 1, 1, 1, GETDATE()),
('Water Pump Installation - Jessore', N'????? ????? ?????? - ????', 'Installed 15 water pumps in rural areas', 'Jessore', 'Jessore Sadar', 'Churamankati', 'Churamankati', 600, 6, 4, 1, 0, 2, GETDATE()),
('Water Pump Installation - Kushtia', N'????? ????? ?????? - ?????????', 'Installed 12 water pumps in rural areas', 'Kushtia', 'Kushtia Sadar', 'Kushtia Sadar', 'Town', 450, 6, 4, 1, 0, 3, GETDATE()),
('Water Pump Installation - Faridpur', N'????? ????? ?????? - ???????', 'Installed 8 water pumps in rural areas', 'Faridpur', 'Faridpur Sadar', 'Char Bhardrashan', 'Char', 350, 6, 4, 1, 0, 4, GETDATE());

GO

PRINT '? Sample events inserted successfully!';
PRINT '';
PRINT 'Statistics Summary:';
PRINT '- Total Programs: 4';
PRINT '- Total Events: 14';
PRINT '- Distinct Districts: 11 (Dhaka, Chittagong, Sylhet, Gazipur, Narayanganj, Comilla, Rajshahi, Khulna, Barisal, Jessore, Kushtia, Faridpur)';
PRINT '- Total Beneficiaries: 3,560';
PRINT '';
PRINT 'Now refresh your home page to see the statistics!';
