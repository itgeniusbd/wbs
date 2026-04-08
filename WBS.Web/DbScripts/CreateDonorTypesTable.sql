-- Create DonorTypes Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DonorTypes')
BEGIN
    CREATE TABLE DonorTypes (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        NameBn NVARCHAR(100) NULL,
        Description NVARCHAR(500) NULL,
        DescriptionBn NVARCHAR(500) NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        IsVisible BIT NOT NULL DEFAULT 1,
        DisplayOrder INT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL
    );

    PRINT 'DonorTypes table created successfully.';
END
ELSE
BEGIN
    PRINT 'DonorTypes table already exists.';
END
GO

-- Insert Default Donor Types
IF NOT EXISTS (SELECT 1 FROM DonorTypes)
BEGIN
    SET IDENTITY_INSERT DonorTypes ON;

    INSERT INTO DonorTypes (Id, Name, NameBn, Description, IsActive, IsVisible, DisplayOrder, CreatedAt)
    VALUES 
        (1, 'Regular', N'?????? ????', 'Regular Donor', 1, 1, 1, GETUTCDATE()),
        (2, 'Monthly', N'????? ????', 'Monthly recurring donor', 1, 1, 2, GETUTCDATE()),
        (3, 'Daily', N'????? ????', 'Daily recurring donor', 1, 1, 3, GETUTCDATE()),
        (4, 'Yearly', N'??????? ????', 'Yearly recurring donor', 1, 1, 4, GETUTCDATE()),
        (5, 'Lifetime', N'???????? ????', 'Lifetime donor', 1, 1, 5, GETUTCDATE()),
        (6, 'Corporate', N'????????????? ????', 'Corporate or institutional donor', 1, 1, 6, GETUTCDATE()),
        (7, 'One Time', N'??????? ????', 'One-time donor', 1, 1, 7, GETUTCDATE());

    SET IDENTITY_INSERT DonorTypes OFF;

    PRINT '7 default donor types inserted successfully.';
END
ELSE
BEGIN
    PRINT 'Donor types already exist. Skipping insert.';
END
GO

-- Verify the data
SELECT * FROM DonorTypes ORDER BY DisplayOrder;
GO
