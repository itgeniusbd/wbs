-- =============================================
-- Complete Fix Script for Volunteer Feature
-- Run this script to fix all database issues
-- =============================================

PRINT '========================================';
PRINT 'Starting Volunteer Feature Database Fix';
PRINT '========================================';
PRINT '';

-- Step 1: Check if SDGProjects table exists
PRINT 'Step 1: Checking SDGProjects table...';
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SDGProjects]') AND type in (N'U'))
BEGIN
    PRINT '? SDGProjects table exists.';
    
    -- Check if required columns exist
    DECLARE @MissingColumns NVARCHAR(MAX) = '';
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SDGProjects]') AND name = 'StartDate')
        SET @MissingColumns = @MissingColumns + 'StartDate, ';
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SDGProjects]') AND name = 'EndDate')
        SET @MissingColumns = @MissingColumns + 'EndDate, ';
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SDGProjects]') AND name = 'IsActive')
        SET @MissingColumns = @MissingColumns + 'IsActive, ';
    
    IF LEN(@MissingColumns) > 0
    BEGIN
        PRINT '? Warning: Missing columns in SDGProjects: ' + LEFT(@MissingColumns, LEN(@MissingColumns) - 1);
    END
    ELSE
    BEGIN
        PRINT '? All required columns exist in SDGProjects table.';
    END
END
ELSE
BEGIN
    PRINT '? Error: SDGProjects table does not exist!';
    PRINT '  Please ensure the database is properly migrated.';
END
PRINT '';

-- Step 2: Add SDGProjectId to Volunteers table
PRINT 'Step 2: Checking Volunteers table for SDGProjectId column...';
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Volunteers]') AND type in (N'U'))
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Volunteers]') AND name = 'SDGProjectId')
    BEGIN
        ALTER TABLE [dbo].[Volunteers] ADD [SDGProjectId] INT NULL;
        PRINT '? SDGProjectId column added to Volunteers table.';
        
        -- Add foreign key constraint
        IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Volunteers_SDGProjects_SDGProjectId')
        BEGIN
            ALTER TABLE [dbo].[Volunteers]
            ADD CONSTRAINT [FK_Volunteers_SDGProjects_SDGProjectId] 
            FOREIGN KEY ([SDGProjectId]) REFERENCES [dbo].[SDGProjects] ([Id]);
            PRINT '? Foreign key constraint added.';
        END
    END
    ELSE
    BEGIN
        PRINT '? SDGProjectId column already exists in Volunteers table.';
        
        -- Check if foreign key exists
        IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Volunteers_SDGProjects_SDGProjectId')
        BEGIN
            ALTER TABLE [dbo].[Volunteers]
            ADD CONSTRAINT [FK_Volunteers_SDGProjects_SDGProjectId] 
            FOREIGN KEY ([SDGProjectId]) REFERENCES [dbo].[SDGProjects] ([Id]);
            PRINT '? Foreign key constraint added.';
        END
        ELSE
        BEGIN
            PRINT '? Foreign key constraint already exists.';
        END
    END
END
ELSE
BEGIN
    PRINT '? Warning: Volunteers table does not exist yet.';
    PRINT '  This is normal if you haven''t run Entity Framework migrations.';
END
PRINT '';

-- Step 3: Insert sample SDG Projects for volunteering (if table is empty)
PRINT 'Step 3: Checking for sample SDG Projects...';
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SDGProjects]') AND type in (N'U'))
BEGIN
    DECLARE @ProjectCount INT;
    SELECT @ProjectCount = COUNT(*) FROM [dbo].[SDGProjects];
    
    IF @ProjectCount = 0
    BEGIN
        PRINT 'Inserting sample SDG Projects for volunteer events...';
        
        SET IDENTITY_INSERT [dbo].[SDGProjects] OFF;
        
        INSERT INTO [dbo].[SDGProjects] 
            ([Title], [TitleBn], [Description], [DescriptionBn], [District], [DistrictBn], 
             [Thana], [ThanaBn], [Village], [VillageBn], [StartDate], [EndDate], 
             [IsActive], [IsFeatured], [DisplayOrder], [BeneficiaryCount])
        VALUES 
        (N'Community Health Fair 2024', N'???????? ????????? ???? ????',
         N'Join us for a free health checkup and consultation event in your community. Our medical team will provide basic health screenings and advice.',
         N'????? ?????????? ?????????? ????????? ??????? ??? ??????? ??????? ??? ???? ?????? ??????? ?? ????? ????????? ????????? ??? ??????? ?????? ?????',
         N'Dhaka', N'????', N'Mirpur', N'??????', N'Mirpur-10', N'??????-??',
         DATEADD(DAY, 15, GETDATE()), DATEADD(DAY, 16, GETDATE()), 1, 1, 1, 500),
        
        (N'Water & Sanitation Workshop', N'???? ? ?????????? ????????',
         N'Learn about proper water management and sanitation practices. This workshop will cover water purification, hygiene, and waste management.',
         N'???? ???? ??????????? ??? ?????????? ??????? ???????? ?????? ?? ?????????? ???? ??????????, ????????????? ??? ?????? ??????????? ???? ??? ????',
         N'Chittagong', N'?????????', N'Patenga', N'????????', N'South Patenga', N'?????? ????????',
         DATEADD(DAY, 20, GETDATE()), DATEADD(DAY, 21, GETDATE()), 1, 0, 2, 300),
        
        (N'Educational Support Program', N'?????? ??????? ????????',
         N'Volunteer to teach underprivileged children in rural areas. Help make a difference in children''s lives through education.',
         N'??????? ??????? ???????????? ??????? ?????? ???????????? ??? ??????? ??????? ??????? ????? ???????? ???? ??????? ?????',
         N'Sylhet', N'?????', N'Companiganj', N'????????????', N'Telikhal', N'???????',
         DATEADD(DAY, 30, GETDATE()), DATEADD(DAY, 60, GETDATE()), 1, 1, 3, 200),
        
        (N'Food Distribution Drive', N'????? ????? ????????',
         N'Help us distribute food packages to families in need. Join our team to make an impact in your community.',
         N'????? ???????????? ????? ??????? ?????? ?????? ??????? ????? ????? ??????????? ?????? ????? ?????? ??? ??? ????',
         N'Dhaka', N'????', N'Mohammadpur', N'???????????', N'Geneva Camp', N'?????? ???????',
         DATEADD(DAY, 10, GETDATE()), DATEADD(DAY, 11, GETDATE()), 1, 1, 4, 1000),
        
        (N'Youth Leadership Training', N'??? ??????? ?????????',
         N'Empower young leaders with skills and knowledge. This training program focuses on leadership, communication, and project management.',
         N'?????? ??? ????? ????? ???? ??????? ????????? ????? ?? ????????? ???????? ???????, ??????? ??? ??????? ????????????? ????? ????',
         N'Rajshahi', N'???????', N'Motihar', N'??????', N'University Area', N'?????????????? ?????',
         DATEADD(DAY, 25, GETDATE()), DATEADD(DAY, 27, GETDATE()), 1, 0, 5, 150);

        PRINT '? 5 sample SDG Projects inserted successfully.';
    END
    ELSE
    BEGIN
        PRINT '? SDGProjects table already has data (' + CAST(@ProjectCount AS VARCHAR) + ' projects found).';
    END
END
PRINT '';

-- Step 4: Verification
PRINT 'Step 4: Verification...';
PRINT '';

-- Check SDGProjects table structure
PRINT 'SDGProjects Table Key Columns:';
SELECT 
    c.name as [Column Name],
    t.name as [Data Type],
    c.is_nullable as [Nullable]
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID(N'[dbo].[SDGProjects]')
  AND c.name IN ('Id', 'Title', 'StartDate', 'EndDate', 'IsActive', 'District', 'Village')
ORDER BY c.column_id;
PRINT '';

-- Check Volunteers table structure
PRINT 'Volunteers Table Key Columns:';
SELECT 
    c.name as [Column Name],
    t.name as [Data Type],
    c.is_nullable as [Nullable]
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID(N'[dbo].[Volunteers]')
  AND c.name IN ('Id', 'FirstName', 'Email', 'SDGProjectId', 'AppliedDate')
ORDER BY c.column_id;
PRINT '';

-- Check sample projects
PRINT 'Sample SDG Projects for Volunteering:';
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SDGProjects]') AND type in (N'U'))
BEGIN
    SELECT TOP 5
        Id,
        Title,
        District,
        Village,
        CONVERT(VARCHAR(10), StartDate, 120) as [Start Date],
        IsActive as [Active]
    FROM [dbo].[SDGProjects]
    WHERE IsActive = 1
    ORDER BY StartDate;
END
PRINT '';

-- Final message
PRINT '========================================';
PRINT '? Database Fix Completed Successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Next Steps:';
PRINT '1. Update your Volunteer model to use SDGProjectId instead of EventId';
PRINT '2. Update your Razor Page to fetch from SDGProjects table';
PRINT '3. Restart your application';
PRINT '4. Navigate to /getinvolved/volunteer';
PRINT '5. Verify that the page loads without errors';
PRINT '6. Check that the SDG Project dropdown shows events';
PRINT '';
PRINT 'Code Changes Required:';
PRINT '- In Volunteer.cs model: Add [ForeignKey("SDGProject")] public int? SDGProjectId { get; set; }';
PRINT '- In Volunteer.cshtml.cs: Change query to use SDGProjects instead of Events';
PRINT '- In Volunteer.cshtml: Update dropdown to show SDGProject.Title and District';
PRINT '';
