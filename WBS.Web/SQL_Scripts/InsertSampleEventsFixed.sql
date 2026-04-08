-- Insert Sample Events/Projects with DYNAMIC Program IDs
-- This will work with your existing programs
USE [WbsDb];
GO

-- First, let's see what programs exist
PRINT '=== Existing Programs ===';
SELECT Id, Title, SDGId FROM SDGPrograms WHERE IsActive = 1;
PRINT '';

-- Declare variables to store program IDs
DECLARE @Program1Id INT, @Program2Id INT, @Program3Id INT, @Program4Id INT;
DECLARE @SDG1 INT, @SDG2 INT, @SDG3 INT, @SDG4 INT;

-- Get the first 4 active programs
SELECT TOP 1 @Program1Id = Id, @SDG1 = SDGId FROM SDGPrograms WHERE IsActive = 1 ORDER BY Id;
SELECT TOP 1 @Program2Id = Id, @SDG2 = SDGId FROM SDGPrograms WHERE IsActive = 1 AND Id > @Program1Id ORDER BY Id;
SELECT TOP 1 @Program3Id = Id, @SDG3 = SDGId FROM SDGPrograms WHERE IsActive = 1 AND Id > @Program2Id ORDER BY Id;
SELECT TOP 1 @Program4Id = Id, @SDG4 = SDGId FROM SDGPrograms WHERE IsActive = 1 AND Id > @Program3Id ORDER BY Id;

PRINT 'Using Program IDs:';
PRINT 'Program 1 ID: ' + CAST(@Program1Id AS VARCHAR) + ' (SDG: ' + CAST(@SDG1 AS VARCHAR) + ')';
PRINT 'Program 2 ID: ' + CAST(@Program2Id AS VARCHAR) + ' (SDG: ' + CAST(@SDG2 AS VARCHAR) + ')';
PRINT 'Program 3 ID: ' + CAST(@Program3Id AS VARCHAR) + ' (SDG: ' + CAST(@SDG3 AS VARCHAR) + ')';
PRINT 'Program 4 ID: ' + CAST(@Program4Id AS VARCHAR) + ' (SDG: ' + CAST(@SDG4 AS VARCHAR) + ')';
PRINT '';

-- Delete existing sample events if any
DELETE FROM SDGProjects WHERE Title LIKE 'Training Center at%' OR Title LIKE 'Sewing Machine Distribution%' OR Title LIKE 'Educational Support%' OR Title LIKE 'Water Pump Installation%';
PRINT '? Cleared old sample events';
PRINT '';

-- Insert events for Program 1
IF @Program1Id IS NOT NULL
BEGIN
    INSERT INTO SDGProjects (Title, TitleBn, Description, District, Thana, [Union], Village, BeneficiaryCount, SDGId, SDGProgramId, IsActive, IsFeatured, DisplayOrder, CreatedAt)
    VALUES 
    ('Training Center at Dhaka', N'?????? ????????? ???????', 'Training center construction in Dhaka', 'Dhaka', 'Mirpur', 'Mirpur-1', 'Block-C', 150, @SDG1, @Program1Id, 1, 1, 1, GETDATE()),
    ('Training Center at Chittagong', N'?????????? ????????? ???????', 'Training center construction in Chittagong', 'Chittagong', 'Patenga', 'North Patenga', 'Patenga', 200, @SDG1, @Program1Id, 1, 0, 2, GETDATE()),
    ('Training Center at Sylhet', N'?????? ????????? ???????', 'Training center construction in Sylhet', 'Sylhet', 'Dakshin Surma', 'Mogolgaon', 'Mogolgaon', 120, @SDG1, @Program1Id, 1, 0, 3, GETDATE());
    PRINT '? Inserted 3 events for Program 1';
END

-- Insert events for Program 2
IF @Program2Id IS NOT NULL
BEGIN
    INSERT INTO SDGProjects (Title, TitleBn, Description, District, Thana, [Union], Village, BeneficiaryCount, SDGId, SDGProgramId, IsActive, IsFeatured, DisplayOrder, CreatedAt)
    VALUES 
    ('Sewing Machine Distribution - Dhaka', N'????? ????? ????? - ????', 'Distributed 50 sewing machines to women', 'Dhaka', 'Uttara', 'Uttara West', 'Sector-7', 50, @SDG2, @Program2Id, 1, 1, 1, GETDATE()),
    ('Sewing Machine Distribution - Gazipur', N'????? ????? ????? - ???????', 'Distributed 75 sewing machines to women', 'Gazipur', 'Gazipur Sadar', 'Tongi', 'Tongi', 75, @SDG2, @Program2Id, 1, 0, 2, GETDATE()),
    ('Sewing Machine Distribution - Narayanganj', N'????? ????? ????? - ???????????', 'Distributed 60 sewing machines to women', 'Narayanganj', 'Narayanganj Sadar', 'Fatullah', 'Fatullah', 60, @SDG2, @Program2Id, 1, 0, 3, GETDATE());
    PRINT '? Inserted 3 events for Program 2';
END

-- Insert events for Program 3
IF @Program3Id IS NOT NULL
BEGIN
    INSERT INTO SDGProjects (Title, TitleBn, Description, District, Thana, [Union], Village, BeneficiaryCount, SDGId, SDGProgramId, IsActive, IsFeatured, DisplayOrder, CreatedAt)
    VALUES 
    ('Educational Support - Dhaka', N'?????? ??????? - ????', 'Educational support for underprivileged children', 'Dhaka', 'Mohammadpur', 'Mohammadpur', 'Block-B', 300, @SDG3, @Program3Id, 1, 1, 1, GETDATE()),
    ('Educational Support - Comilla', N'?????? ??????? - ????????', 'Educational support for underprivileged children', 'Comilla', 'Comilla Sadar', 'Comilla Sadar', 'Town', 250, @SDG3, @Program3Id, 1, 0, 2, GETDATE()),
    ('Educational Support - Rajshahi', N'?????? ??????? - ???????', 'Educational support for underprivileged children', 'Rajshahi', 'Rajshahi Sadar', 'Rajpara', 'Rajpara', 180, @SDG3, @Program3Id, 1, 0, 3, GETDATE()),
    ('Educational Support - Khulna', N'?????? ??????? - ?????', 'Educational support for underprivileged children', 'Khulna', 'Khulna Sadar', 'Sonadanga', 'Sonadanga', 220, @SDG3, @Program3Id, 1, 0, 4, GETDATE());
    PRINT '? Inserted 4 events for Program 3';
END

-- Insert events for Program 4
IF @Program4Id IS NOT NULL
BEGIN
    INSERT INTO SDGProjects (Title, TitleBn, Description, District, Thana, [Union], Village, BeneficiaryCount, SDGId, SDGProgramId, IsActive, IsFeatured, DisplayOrder, CreatedAt)
    VALUES 
    ('Water Pump Installation - Barisal', N'????? ????? ?????? - ??????', 'Installed 10 water pumps in rural areas', 'Barisal', 'Barisal Sadar', 'Chandmari', 'Chandmari', 500, @SDG4, @Program4Id, 1, 1, 1, GETDATE()),
    ('Water Pump Installation - Jessore', N'????? ????? ?????? - ????', 'Installed 15 water pumps in rural areas', 'Jessore', 'Jessore Sadar', 'Churamankati', 'Churamankati', 600, @SDG4, @Program4Id, 1, 0, 2, GETDATE()),
    ('Water Pump Installation - Kushtia', N'????? ????? ?????? - ?????????', 'Installed 12 water pumps in rural areas', 'Kushtia', 'Kushtia Sadar', 'Kushtia Sadar', 'Town', 450, @SDG4, @Program4Id, 1, 0, 3, GETDATE()),
    ('Water Pump Installation - Faridpur', N'????? ????? ?????? - ???????', 'Installed 8 water pumps in rural areas', 'Faridpur', 'Faridpur Sadar', 'Char Bhardrashan', 'Char', 350, @SDG4, @Program4Id, 1, 0, 4, GETDATE());
    PRINT '? Inserted 4 events for Program 4';
END

GO

PRINT '';
PRINT '=== SUMMARY ===';
PRINT 'Total Events Inserted: ' + CAST((SELECT COUNT(*) FROM SDGProjects WHERE IsActive = 1) AS VARCHAR);
PRINT 'Distinct Districts: ' + CAST((SELECT COUNT(DISTINCT District) FROM SDGProjects WHERE IsActive = 1 AND District IS NOT NULL) AS VARCHAR);
PRINT 'Distinct Thanas: ' + CAST((SELECT COUNT(DISTINCT Thana) FROM SDGProjects WHERE IsActive = 1 AND Thana IS NOT NULL) AS VARCHAR);
PRINT 'Total Beneficiaries: ' + CAST((SELECT SUM(BeneficiaryCount) FROM SDGProjects WHERE IsActive = 1) AS VARCHAR);
PRINT '';
PRINT '? Sample events inserted successfully!';
PRINT 'Now RESTART your application and refresh the home page!';
