-- Check if AnnualReports table exists and what columns it has
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'AnnualReports')
BEGIN
    PRINT 'AnnualReports table exists. Checking columns...'
    
    -- Check for CreatedAt column
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'CreatedAt')
    BEGIN
        PRINT 'Adding CreatedAt column...'
        ALTER TABLE AnnualReports ADD CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE()
    END
    
    -- Check for Description column
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'Description')
    BEGIN
        PRINT 'Adding Description column...'
        ALTER TABLE AnnualReports ADD Description nvarchar(max) NULL
    END
    
    -- Check for DescriptionBn column
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'DescriptionBn')
    BEGIN
        PRINT 'Adding DescriptionBn column...'
        ALTER TABLE AnnualReports ADD DescriptionBn nvarchar(max) NULL
    END
    
    -- Check for CoverImage column
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'CoverImage')
    BEGIN
        PRINT 'Adding CoverImage column...'
        ALTER TABLE AnnualReports ADD CoverImage nvarchar(max) NULL
    END
    
    -- Check for IsActive column
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AnnualReports') AND name = 'IsActive')
    BEGIN
        PRINT 'Adding IsActive column...'
        ALTER TABLE AnnualReports ADD IsActive bit NOT NULL DEFAULT 1
    END
    
    PRINT 'AnnualReports table structure fixed successfully!'
END
ELSE
BEGIN
    PRINT 'AnnualReports table does not exist. Creating it...'
    CREATE TABLE AnnualReports (
        Id int NOT NULL IDENTITY(1,1),
        Title nvarchar(200) NOT NULL,
        TitleBn nvarchar(200) NULL,
        Year int NOT NULL,
        Description nvarchar(max) NULL,
        DescriptionBn nvarchar(max) NULL,
        CoverImage nvarchar(max) NULL,
        FileUrl nvarchar(max) NOT NULL,
        IsActive bit NOT NULL DEFAULT 1,
        CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT PK_AnnualReports PRIMARY KEY (Id)
    )
    PRINT 'AnnualReports table created successfully!'
END
GO
