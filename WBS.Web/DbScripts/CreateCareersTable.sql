-- SQL Script to Create/Update Careers Table
-- Run this script in your database

-- Check if table exists and drop it (optional - only if you want to recreate)
-- DROP TABLE IF EXISTS [dbo].[Careers];

-- Create Careers table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Careers] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Title] NVARCHAR(300) NOT NULL,
        [TitleBn] NVARCHAR(300) NULL,
        [Slug] NVARCHAR(300) NOT NULL,
        [Department] NVARCHAR(MAX) NULL,
        [Location] NVARCHAR(MAX) NULL,
        [JobType] NVARCHAR(MAX) NULL,
        [Description] NVARCHAR(MAX) NULL,
        [DescriptionBn] NVARCHAR(MAX) NULL,
        [Requirements] NVARCHAR(MAX) NULL,
        [RequirementsBn] NVARCHAR(MAX) NULL,
        [Benefits] NVARCHAR(MAX) NULL,
        [SalaryRange] NVARCHAR(MAX) NULL,
        [Deadline] DATETIME2 NULL,
        [ApplicationUrl] NVARCHAR(MAX) NULL,
        [ApplicationEmail] NVARCHAR(MAX) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 NULL
    );
    
    PRINT 'Careers table created successfully';
END
ELSE
BEGIN
    PRINT 'Careers table already exists';
    
    -- Add missing columns if table exists but columns are missing
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'ApplicationEmail')
    BEGIN
        ALTER TABLE [dbo].[Careers] ADD [ApplicationEmail] NVARCHAR(MAX) NULL;
        PRINT 'Added ApplicationEmail column';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'ApplicationUrl')
    BEGIN
        ALTER TABLE [dbo].[Careers] ADD [ApplicationUrl] NVARCHAR(MAX) NULL;
        PRINT 'Added ApplicationUrl column';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'Benefits')
    BEGIN
        ALTER TABLE [dbo].[Careers] ADD [Benefits] NVARCHAR(MAX) NULL;
        PRINT 'Added Benefits column';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'CreatedAt')
    BEGIN
        ALTER TABLE [dbo].[Careers] ADD [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE();
        PRINT 'Added CreatedAt column';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'Department')
    BEGIN
        ALTER TABLE [dbo].[Careers] ADD [Department] NVARCHAR(MAX) NULL;
        PRINT 'Added Department column';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'Requirements')
    BEGIN
        ALTER TABLE [dbo].[Careers] ADD [Requirements] NVARCHAR(MAX) NULL;
        PRINT 'Added Requirements column';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'RequirementsBn')
    BEGIN
        ALTER TABLE [dbo].[Careers] ADD [RequirementsBn] NVARCHAR(MAX) NULL;
        PRINT 'Added RequirementsBn column';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'SalaryRange')
    BEGIN
        ALTER TABLE [dbo].[Careers] ADD [SalaryRange] NVARCHAR(MAX) NULL;
        PRINT 'Added SalaryRange column';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'Slug')
    BEGIN
        ALTER TABLE [dbo].[Careers] ADD [Slug] NVARCHAR(300) NOT NULL DEFAULT '';
        PRINT 'Added Slug column';
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND name = 'UpdatedAt')
    BEGIN
        ALTER TABLE [dbo].[Careers] ADD [UpdatedAt] DATETIME2 NULL;
        PRINT 'Added UpdatedAt column';
    END
END

-- Insert sample career data (optional)
IF NOT EXISTS (SELECT * FROM [dbo].[Careers])
BEGIN
    INSERT INTO [dbo].[Careers] 
        ([Title], [TitleBn], [Slug], [Department], [Location], [JobType], [Description], [DescriptionBn], 
         [Requirements], [RequirementsBn], [Benefits], [SalaryRange], [Deadline], [ApplicationEmail], [IsActive], [CreatedAt])
    VALUES 
        (N'Program Manager', N'????????? ?????????', 'program-manager', 
         N'Programs', N'Dhaka, Bangladesh', N'Full-time',
         N'We are looking for an experienced Program Manager to lead our development programs.',
         N'???? ?????? ??????? ????????????? ????????? ???? ???? ?????? ????????? ????????? ???????',
         N'<ul><li>Bachelor''s degree in relevant field</li><li>5+ years of experience in program management</li><li>Strong leadership skills</li><li>Excellent communication in English and Bangla</li></ul>',
         N'<ul><li>?????????? ???????? ?????? ??????</li><li>????????? ????????????? ?+ ????? ????????</li><li>????????? ??????? ??????</li><li>?????? ??? ??????? ?????? ???????</li></ul>',
         N'Competitive salary, health insurance, annual bonus, professional development opportunities',
         N'?40,000 - ?60,000',
         DATEADD(month, 1, GETDATE()),
         N'careers@wbs.org',
         1,
         GETUTCDATE()),
         
        (N'Field Officer', N'????? ??????', 'field-officer',
         N'Field Operations', N'Sylhet, Bangladesh', N'Full-time',
         N'Join our team as a Field Officer to implement community development projects.',
         N'???????? ??????? ??????? ???????????? ???? ????? ?????? ?????? ?????? ??? ??? ????',
         N'<ul><li>Bachelor''s degree</li><li>2+ years field experience</li><li>Ability to work in rural areas</li><li>Good interpersonal skills</li></ul>',
         N'<ul><li>?????? ??????</li><li>?+ ????? ????? ????????</li><li>??????? ??????? ??? ???? ??????</li><li>???? ????????????? ??????</li></ul>',
         N'Health insurance, travel allowance, training opportunities',
         N'?25,000 - ?35,000',
         DATEADD(month, 2, GETDATE()),
         N'careers@wbs.org',
         1,
         GETUTCDATE());
    
    PRINT 'Sample career data inserted';
END

PRINT 'Careers table setup completed successfully!';
