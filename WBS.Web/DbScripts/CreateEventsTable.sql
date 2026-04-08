-- Check if Events table exists and create if not
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Events' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Events] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Title] NVARCHAR(300) NOT NULL,
        [TitleBn] NVARCHAR(300) NULL,
        [Slug] NVARCHAR(300) NOT NULL,
        [Description] NVARCHAR(MAX) NULL,
        [DescriptionBn] NVARCHAR(MAX) NULL,
        [FeaturedImage] NVARCHAR(500) NULL,
        [Location] NVARCHAR(200) NULL,
        [LocationBn] NVARCHAR(200) NULL,
        [StartDate] DATETIME2 NOT NULL,
        [EndDate] DATETIME2 NULL,
        [RegistrationUrl] NVARCHAR(500) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [IsFeatured] BIT NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );

    PRINT 'Events table created successfully.';
END
ELSE
BEGIN
    PRINT 'Events table already exists.';
    
    -- Check and add missing columns if table exists
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'TitleBn')
    BEGIN
        ALTER TABLE [dbo].[Events] ADD [TitleBn] NVARCHAR(300) NULL;
        PRINT 'TitleBn column added to Events table.';
    END

    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'DescriptionBn')
    BEGIN
        ALTER TABLE [dbo].[Events] ADD [DescriptionBn] NVARCHAR(MAX) NULL;
        PRINT 'DescriptionBn column added to Events table.';
    END

    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'LocationBn')
    BEGIN
        ALTER TABLE [dbo].[Events] ADD [LocationBn] NVARCHAR(200) NULL;
        PRINT 'LocationBn column added to Events table.';
    END
END
GO

-- Insert sample events if table is empty
IF NOT EXISTS (SELECT * FROM [dbo].[Events])
BEGIN
    INSERT INTO [dbo].[Events] ([Title], [TitleBn], [Slug], [Description], [DescriptionBn], [Location], [LocationBn], [StartDate], [EndDate], [IsActive], [IsFeatured])
    VALUES 
    ('Community Health Fair 2024', '???????? ????????? ???? ????', 'community-health-fair-2024', 
     'Join us for a free health checkup and consultation event in your community.', 
     '????? ?????????? ?????????? ????????? ??????? ??? ??????? ??????? ??? ????',
     'Dhaka, Bangladesh', '????, ????????', 
     DATEADD(DAY, 15, GETDATE()), DATEADD(DAY, 16, GETDATE()), 1, 1),
    
    ('Water & Sanitation Workshop', '???? ? ?????????? ????????', 'water-sanitation-workshop',
     'Learn about proper water management and sanitation practices.',
     '???? ???? ??????????? ??? ?????????? ??????? ???????? ??????',
     'Chittagong, Bangladesh', '?????????, ????????',
     DATEADD(DAY, 20, GETDATE()), DATEADD(DAY, 21, GETDATE()), 1, 0),
    
    ('Educational Support Program', '?????? ??????? ????????', 'educational-support-program',
     'Volunteer to teach underprivileged children in rural areas.',
     '??????? ??????? ???????????? ??????? ?????? ???????????? ???',
     'Sylhet, Bangladesh', '?????, ????????',
     DATEADD(DAY, 30, GETDATE()), DATEADD(DAY, 60, GETDATE()), 1, 1);

    PRINT 'Sample events inserted successfully.';
END
GO
