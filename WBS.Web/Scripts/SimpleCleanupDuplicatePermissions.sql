-- Simple Duplicate Permissions Cleanup Script
-- This is a safer, simpler version that does the cleanup step by step

-- ============================================
-- STEP 1: CHECK CURRENT STATE
-- ============================================
PRINT '============================================';
PRINT 'STEP 1: Checking current state';
PRINT '============================================';
PRINT '';

DECLARE @TotalPermissions INT;
SELECT @TotalPermissions = COUNT(*) FROM Permissions;
PRINT 'Total permissions in database: ' + CAST(@TotalPermissions AS VARCHAR(10));

DECLARE @DuplicateCount INT;
SELECT @DuplicateCount = COUNT(*)
FROM (
    SELECT Module, Action
    FROM Permissions
    GROUP BY Module, Action
    HAVING COUNT(*) > 1
) AS Duplicates;

PRINT 'Number of duplicate permission types: ' + CAST(@DuplicateCount AS VARCHAR(10));

IF @DuplicateCount = 0
BEGIN
    PRINT '';
    PRINT '? No duplicates found! Database is clean.';
    RETURN;
END

PRINT '';
PRINT 'Duplicates found. Starting cleanup...';
PRINT '';

-- ============================================
-- STEP 2: CREATE BACKUP TABLE (Optional)
-- ============================================
PRINT '============================================';
PRINT 'STEP 2: Creating backup (optional)';
PRINT '============================================';
PRINT '';

IF OBJECT_ID('Permissions_Backup_BeforeCleanup') IS NOT NULL
    DROP TABLE Permissions_Backup_BeforeCleanup;

SELECT * 
INTO Permissions_Backup_BeforeCleanup
FROM Permissions;

PRINT '? Backup table created: Permissions_Backup_BeforeCleanup';
PRINT '';

-- ============================================
-- STEP 3: IDENTIFY PERMISSIONS TO KEEP
-- ============================================
PRINT '============================================';
PRINT 'STEP 3: Identifying permissions to keep';
PRINT '============================================';
PRINT '';

-- Create temp table with the IDs we want to keep (lowest ID for each Module-Action pair)
IF OBJECT_ID('tempdb..#PermissionsToKeep') IS NOT NULL 
    DROP TABLE #PermissionsToKeep;

SELECT 
    MIN(Id) as IdToKeep,
    Module,
    Action,
    Name
INTO #PermissionsToKeep
FROM Permissions
GROUP BY Module, Action, Name;

DECLARE @KeepCount INT;
SELECT @KeepCount = COUNT(*) FROM #PermissionsToKeep;
PRINT 'Permissions to keep: ' + CAST(@KeepCount AS VARCHAR(10));

-- ============================================
-- STEP 4: IDENTIFY PERMISSIONS TO DELETE
-- ============================================
PRINT '============================================';
PRINT 'STEP 4: Identifying permissions to delete';
PRINT '============================================';
PRINT '';

IF OBJECT_ID('tempdb..#PermissionsToDelete') IS NOT NULL 
    DROP TABLE #PermissionsToDelete;

SELECT p.Id, p.Module, p.Action, p.Name
INTO #PermissionsToDelete
FROM Permissions p
WHERE NOT EXISTS (
    SELECT 1 
    FROM #PermissionsToKeep ptk 
    WHERE ptk.IdToKeep = p.Id
);

DECLARE @DeleteCount INT;
SELECT @DeleteCount = COUNT(*) FROM #PermissionsToDelete;
PRINT 'Permissions to delete: ' + CAST(@DeleteCount AS VARCHAR(10));

IF @DeleteCount = 0
BEGIN
    PRINT '';
    PRINT '? No duplicate permissions to delete!';
    DROP TABLE #PermissionsToKeep;
    DROP TABLE #PermissionsToDelete;
    RETURN;
END

PRINT '';
SELECT 
    Module,
    Action,
    COUNT(*) as DeleteCount
FROM #PermissionsToDelete
GROUP BY Module, Action
ORDER BY Module, Action;

-- ============================================
-- STEP 5: UPDATE ROLEPERMISSIONS REFERENCES
-- ============================================
PRINT '';
PRINT '============================================';
PRINT 'STEP 5: Updating RolePermissions references';
PRINT '============================================';
PRINT '';

BEGIN TRANSACTION;

BEGIN TRY
    -- Update RolePermissions to point to the kept permissions
    UPDATE rp
    SET rp.PermissionId = ptk.IdToKeep
    FROM RolePermissions rp
    INNER JOIN #PermissionsToDelete ptd ON rp.PermissionId = ptd.Id
    INNER JOIN #PermissionsToKeep ptk 
        ON ptd.Module = ptk.Module 
        AND ptd.Action = ptk.Action
        AND ptd.Name = ptk.Name;

    DECLARE @UpdatedCount INT = @@ROWCOUNT;
    PRINT '? Updated ' + CAST(@UpdatedCount AS VARCHAR(10)) + ' RolePermissions references';

    -- ============================================
    -- STEP 6: REMOVE DUPLICATE ROLEPERMISSIONS
    -- ============================================
    PRINT '';
    PRINT '============================================';
    PRINT 'STEP 6: Removing duplicate RolePermissions';
    PRINT '============================================';
    PRINT '';

    -- Delete duplicate RolePermissions (same RoleId-PermissionId combination)
    DELETE FROM RolePermissions
    WHERE EXISTS (
        SELECT 1
        FROM (
            SELECT 
                RoleId,
                PermissionId,
                ROW_NUMBER() OVER (PARTITION BY RoleId, PermissionId ORDER BY (SELECT NULL)) as RowNum
            FROM RolePermissions
        ) AS rp_numbered
        WHERE rp_numbered.RoleId = RolePermissions.RoleId
        AND rp_numbered.PermissionId = RolePermissions.PermissionId
        AND rp_numbered.RowNum > 1
    );

    DECLARE @DeletedRP INT = @@ROWCOUNT;
    PRINT '? Deleted ' + CAST(@DeletedRP AS VARCHAR(10)) + ' duplicate RolePermissions';

    -- ============================================
    -- STEP 7: DELETE DUPLICATE PERMISSIONS
    -- ============================================
    PRINT '';
    PRINT '============================================';
    PRINT 'STEP 7: Deleting duplicate permissions';
    PRINT '============================================';
    PRINT '';

    DELETE FROM Permissions
    WHERE Id IN (SELECT Id FROM #PermissionsToDelete);

    DECLARE @DeletedPermissions INT = @@ROWCOUNT;
    PRINT '? Deleted ' + CAST(@DeletedPermissions AS VARCHAR(10)) + ' duplicate permissions';

    -- ============================================
    -- STEP 8: VERIFICATION
    -- ============================================
    PRINT '';
    PRINT '============================================';
    PRINT 'STEP 8: Verifying cleanup';
    PRINT '============================================';
    PRINT '';

    DECLARE @RemainingDuplicates INT;
    SELECT @RemainingDuplicates = COUNT(*)
    FROM (
        SELECT Module, Action
        FROM Permissions
        GROUP BY Module, Action
        HAVING COUNT(*) > 1
    ) AS StillDuplicates;

    IF @RemainingDuplicates = 0
    BEGIN
        COMMIT TRANSACTION;
        
        PRINT '??? SUCCESS! All duplicates cleaned up! ???';
        PRINT '';
        PRINT '=== SUMMARY ===';
        PRINT 'Original permissions: ' + CAST(@TotalPermissions AS VARCHAR(10));
        PRINT 'Deleted permissions: ' + CAST(@DeletedPermissions AS VARCHAR(10));
        PRINT 'Updated RolePermissions: ' + CAST(@UpdatedCount AS VARCHAR(10));
        PRINT 'Deleted duplicate RolePermissions: ' + CAST(@DeletedRP AS VARCHAR(10));
        
        DECLARE @FinalCount INT;
        SELECT @FinalCount = COUNT(*) FROM Permissions;
        PRINT 'Final permission count: ' + CAST(@FinalCount AS VARCHAR(10));
        PRINT '';
        PRINT '? Transaction committed successfully!';
        PRINT '';
        PRINT 'Note: Backup table "Permissions_Backup_BeforeCleanup" is available if needed.';
        PRINT 'You can drop it with: DROP TABLE Permissions_Backup_BeforeCleanup';
    END
    ELSE
    BEGIN
        ROLLBACK TRANSACTION;
        PRINT '??? ERROR: Still found ' + CAST(@RemainingDuplicates AS VARCHAR(10)) + ' duplicates!';
        PRINT '? Transaction rolled back.';
        PRINT '';
        PRINT 'Remaining duplicates:';
        SELECT Module, Action, COUNT(*) as Count
        FROM Permissions
        GROUP BY Module, Action
        HAVING COUNT(*) > 1;
    END

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    
    PRINT '';
    PRINT '??? ERROR OCCURRED! ???';
    PRINT '? Transaction rolled back - no changes made.';
    PRINT '';
    PRINT 'Error details:';
    PRINT 'Error Number: ' + CAST(ERROR_NUMBER() AS VARCHAR(10));
    PRINT 'Error Message: ' + ERROR_MESSAGE();
    PRINT 'Error Line: ' + CAST(ERROR_LINE() AS VARCHAR(10));
END CATCH

-- Cleanup temp tables
IF OBJECT_ID('tempdb..#PermissionsToKeep') IS NOT NULL 
    DROP TABLE #PermissionsToKeep;
IF OBJECT_ID('tempdb..#PermissionsToDelete') IS NOT NULL 
    DROP TABLE #PermissionsToDelete;

PRINT '';
PRINT '============================================';
PRINT 'Cleanup script completed';
PRINT '============================================';
