-- Update Site Settings with new Contact Information
-- Run this script to update the contact details in the database

USE [WBS]
GO

-- Check if SiteSettings table has any records
IF EXISTS (SELECT 1 FROM [dbo].[SiteSettings])
BEGIN
    -- Update existing record
    UPDATE [dbo].[SiteSettings]
    SET 
        [Address] = 'House - 15 (5A), Road-08, Block -C, Bosila Garden City, Mohammadpur, Dhaka-1207',
        [AddressBn] = '???? - ?? (??), ???-??, ???? -??, ????? ??????? ????, ???????????, ????-????',
        [Phone] = '+8801550721313',
        [Email] = 'info@wbs-bd.org',
        [UpdatedAt] = GETUTCDATE()
    WHERE [Id] = (SELECT TOP 1 [Id] FROM [dbo].[SiteSettings])

    PRINT 'Site settings updated successfully!'
END
ELSE
BEGIN
    -- Insert new record if table is empty
    INSERT INTO [dbo].[SiteSettings] 
    (
        [SiteName], 
        [SiteNameBn],
        [Address], 
        [AddressBn],
        [Phone], 
        [Email],
        [UpdatedAt]
    )
    VALUES 
    (
        'Working Bangladesh Society (WBS)',
        '????????? ???????? ??????? (??????????)',
        'House - 15 (5A), Road-08, Block -C, Bosila Garden City, Mohammadpur, Dhaka-1207',
        '???? - ?? (??), ???-??, ???? -??, ????? ??????? ????, ???????????, ????-????',
        '+8801550721313',
        'info@wbs-bd.org',
        GETUTCDATE()
    )

    PRINT 'Site settings created successfully!'
END
GO

-- Verify the update
SELECT 
    [SiteName],
    [Address],
    [Phone],
    [Email],
    [UpdatedAt]
FROM [dbo].[SiteSettings]
GO
