-- Add Permissions for Users and Notification Templates (SMS & Email Templates)
-- Run this script to add only these two module permissions

BEGIN TRANSACTION;

BEGIN TRY
    PRINT '========================================';
    PRINT 'Adding Users and Notification Templates Permissions';
    PRINT '========================================';
    PRINT '';
    
    -- Get current max DisplayOrder
    DECLARE @MaxOrder INT;
    SELECT @MaxOrder = ISNULL(MAX(DisplayOrder), 0) FROM Permissions;
    PRINT 'Current max DisplayOrder: ' + CAST(@MaxOrder AS VARCHAR(10));
    PRINT '';
    
    -- Check if permissions already exist
    DECLARE @UsersCount INT, @TemplatesCount INT;
    SELECT @UsersCount = COUNT(*) FROM Permissions WHERE Module = 'Users';
    SELECT @TemplatesCount = COUNT(*) FROM Permissions WHERE Module = 'Notification Templates';
    
    IF @UsersCount > 0
    BEGIN
        PRINT '⚠️ WARNING: Users module already has ' + CAST(@UsersCount AS VARCHAR(10)) + ' permissions';
        PRINT 'Skipping Users permissions...';
    END
    ELSE
    BEGIN
        -- Users Management
        PRINT 'Adding Users Management permissions...';
        INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
        VALUES 
            ('View Users', 'ব্যবহারকারী দেখুন', 'Users', 'View', 'View Users', @MaxOrder + 1),
            ('Create Users', 'ব্যবহারকারী তৈরি করুন', 'Users', 'Create', 'Create Users', @MaxOrder + 2),
            ('Edit Users', 'ব্যবহারকারী সম্পাদনা করুন', 'Users', 'Edit', 'Edit Users', @MaxOrder + 3),
            ('Delete Users', 'ব্যবহারকারী মুছুন', 'Users', 'Delete', 'Delete Users', @MaxOrder + 4);
        
        PRINT '✓ Added 4 Users permissions';
        SET @MaxOrder = @MaxOrder + 4;
    END
    
    PRINT '';
    
    IF @TemplatesCount > 0
    BEGIN
        PRINT '⚠️ WARNING: Notification Templates already has ' + CAST(@TemplatesCount AS VARCHAR(10)) + ' permissions';
        PRINT 'Skipping Notification Templates permissions...';
    END
    ELSE
    BEGIN
        -- Notification Templates (SMS & Email Templates)
        PRINT 'Adding Notification Templates (SMS & Email) permissions...';
        INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
        VALUES 
            ('View Notification Templates', 'বিজ্ঞপ্তি টেমপ্লেট দেখুন', 'Notification Templates', 'View', 'View Notification Templates', @MaxOrder + 1),
            ('Create Notification Templates', 'বিজ্ঞপ্তি টেমপ্লেট তৈরি করুন', 'Notification Templates', 'Create', 'Create Notification Templates', @MaxOrder + 2),
            ('Edit Notification Templates', 'বিজ্ঞপ্তি টেমপ্লেট সম্পাদনা করুন', 'Notification Templates', 'Edit', 'Edit Notification Templates', @MaxOrder + 3),
            ('Delete Notification Templates', 'বিজ্ঞপ্তি টেমপ্লেট মুছুন', 'Notification Templates', 'Delete', 'Delete Notification Templates', @MaxOrder + 4);
        
        PRINT '✓ Added 4 Notification Templates permissions';
    END
    
    PRINT '';
    PRINT '========================================';
    PRINT 'Summary';
    PRINT '========================================';
    
    -- Show final counts
    SELECT @UsersCount = COUNT(*) FROM Permissions WHERE Module = 'Users';
    SELECT @TemplatesCount = COUNT(*) FROM Permissions WHERE Module = 'Notification Templates';
    
    PRINT 'Total Users permissions: ' + CAST(@UsersCount AS VARCHAR(10));
    PRINT 'Total Notification Templates permissions: ' + CAST(@TemplatesCount AS VARCHAR(10));
    
    DECLARE @TotalPermissions INT;
    SELECT @TotalPermissions = COUNT(*) FROM Permissions;
    PRINT 'Total permissions in database: ' + CAST(@TotalPermissions AS VARCHAR(10));
    
    PRINT '';
    PRINT '✓✓✓ SUCCESS! Permissions added successfully! ✓✓✓';
    
    COMMIT TRANSACTION;
    
    PRINT '';
    PRINT '✓ Transaction committed.';
    PRINT '';
    PRINT '========================================';
    PRINT 'Next Steps:';
    PRINT '========================================';
    PRINT '1. Go to Admin → Roles Management';
    PRINT '2. Edit each role and assign new permissions';
    PRINT '3. Test access with different roles';
    PRINT '';
    
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    
    PRINT '';
    PRINT '✗✗✗ ERROR OCCURRED! ✗✗✗';
    PRINT '✗ Transaction rolled back - no changes made.';
    PRINT '';
    PRINT 'Error details:';
    PRINT 'Error Number: ' + CAST(ERROR_NUMBER() AS VARCHAR(10));
    PRINT 'Error Message: ' + ERROR_MESSAGE();
    PRINT 'Error Line: ' + CAST(ERROR_LINE() AS VARCHAR(10));
    PRINT '';
END CATCH

-- Show the new permissions
PRINT '';
PRINT '========================================';
PRINT 'New Permissions Details:';
PRINT '========================================';
PRINT '';

SELECT 
    Id,
    Name,
    NameBn,
    Module,
    Action,
    DisplayOrder
FROM Permissions
WHERE Module IN ('Users', 'Notification Templates')
ORDER BY Module, DisplayOrder;

PRINT '';
PRINT 'Script completed.';
